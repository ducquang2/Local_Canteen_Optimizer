using Local_Canteen_Optimizer.Commands;
using Local_Canteen_Optimizer.DAO.DiscountDAO;
using Local_Canteen_Optimizer.DAO.OrderDAO;
using Local_Canteen_Optimizer.DAO.SeatDAO;
using Local_Canteen_Optimizer.Helper;
using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Local_Canteen_Optimizer.ViewModel
{
    /// <summary>
    /// ViewModel for managing the cart in the Local Canteen Optimizer application.
    /// </summary>
    public class CartViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Data Access Object (DAO) for interacting with order data.
        /// </summary>
        private IOrderDAO _dao = null;

        /// <summary>
        /// Collection of items in the cart.
        /// </summary>
        public ObservableCollection<FoodModel> CartItems { get; set; }

        /// <summary>
        /// Command to remove an item from the cart.
        /// </summary>
        public ICommand RemoveItemCommand { get; set; }

        public double Subtotal { get; set; } = 0;
        public double Tax => Subtotal * 0;

        /// <summary>
        /// Gets or sets the order ID to check new order or old order.
        /// </summary>
        public int OrderId { get; set; } = 0;

        private int selectedTableId;
        public int SelectedTableId
        {
            get => selectedTableId;
            set
            {
                selectedTableId = value;
                OnPropertyChanged(nameof(SelectedTableId));
                OnPropertyChanged(nameof(DisplayTableText));
                LoadOrderItemsAsync();
            }
        }
        public string DisplayTableText => SelectedTableId == 1 ? "Mang về" : $"Bàn {SelectedTableId}";

        private string note;
        public string Note
        {
            get => note;
            set
            {
                note = value;
                OnPropertyChanged(nameof(Note));
            }
        }

        private DiscountModel _selectedDiscount;
        public DiscountModel SelectedDiscount
        {
            get => _selectedDiscount;
            set
            {
                _selectedDiscount = value;
                OnPropertyChanged(nameof(SelectedDiscount));
                OnPropertyChanged(nameof(DiscountText));
                OnPropertyChanged(nameof(DiscountAmount));
                OnPropertyChanged(nameof(Total));
            }
        }

        /// <summary>
        /// Gets the discount amount.
        /// </summary>
        public double DiscountAmount => SelectedDiscount != null ? SelectedDiscount.DiscountAmount : 0 ;
        public string DiscountText => SelectedDiscount != null
        ? $"{SelectedDiscount.DiscountName} - {SelectedDiscount.DiscountDescription}"
        : "No discount applied";

        /// <summary>
        /// Gets the total amount after applying discounts and points.
        /// </summary>
        public double Total => Subtotal - DiscountAmount - PointsToUse;

        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        private CustomerModel _customer;
        public CustomerModel Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged(nameof(Customer));
                OnPropertyChanged(nameof(IsCustomerFound));
            }
        }
        public bool IsCustomerFound => Customer != null;

        /// <summary>
        /// Gets or sets the points to use.
        /// </summary>
        private int _pointsToUse = 0;
        public int PointsToUse
        {
            get => _pointsToUse;
            set
            {
                if (_pointsToUse != value)
                {
                    _pointsToUse = value;
                    OnPropertyChanged(nameof(PointsToUse));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartViewModel"/> class.
        /// </summary>
        public CartViewModel()
        {
            _dao = new OrderDAOImp();
            selectedTableId = 1;
            CartItems = new ObservableCollection<FoodModel>();
            //RemoveItemCommand = new RelayCommand<FoodModel>(RemoveItem);
            LoadOrderItemsAsync();
        }

        /// <summary>
        /// Loads all order items by table with asynchronously.
        /// </summary>
        public async Task LoadOrderItemsAsync()
        {
            OrderModel orderModel = await _dao.GetOrderModelFromTable(selectedTableId);
            CartItems.Clear();
            if(orderModel == null)
            {
                OrderId = 0;
                OnPropertyChanged(nameof(CartItems));
                Subtotal = 0;
                SelectedDiscount = null;
                Customer = null;
                OnPropertyChanged(nameof(Subtotal));
                OnPropertyChanged(nameof(Tax));
                OnPropertyChanged(nameof(Total));
                OnPropertyChanged(nameof(Note));
                return;
            }
            OrderId = orderModel.OrderId;
            foreach (var item in orderModel.OrderDetails)
            {
                CartItems.Add(item);
            }
            Subtotal = CartItems.Sum(i => i.Price * i.QuantityBuy);
            Note = orderModel.Note;
            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(Note));
        }

        public async Task<bool> UpdateOrderNoteAsync()
        {
            try
            {
                var order = await _dao.GetOrderModelFromTable(SelectedTableId);
                if (order != null)
                {
                    order.Note = Note;
                    return await _dao.UpdateOrder(order);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Adds an item to the cart.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddItemToCart(FoodModel item)
        {
            FoodModel existingItem = CartItems.FirstOrDefault(i => i.ProductID == item.ProductID);
            if (existingItem != null)
            {
                // Tăng số lượng nếu đã tồn tại
                existingItem.QuantityBuy += 1;
                Subtotal += existingItem.Price;
                //int index = CartItems.IndexOf(existingItem);
                //CartItems[index] = existingItem;
            }
            else
            {
                item.QuantityBuy = 1;
                CartItems.Add(item);
                Subtotal += item.Price;
            }
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }

        /// <summary>
        /// Removes an item from the cart.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public void RemoveItem(FoodModel item)
        {
            CartItems.Remove(item);
            Subtotal -= item.Price * item.QuantityBuy;
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }

        /// <summary>
        /// Holds the current cart asynchronously.
        /// </summary>
        /// <returns>The table information after holding the cart.</returns>
        public async Task<TableModel> HoldCart()
        {
            var order = new OrderModel
            {
                OrderId = OrderId,
                //TableNumber = 1,
                //OrderTime = DateTime.Now,
                OrderDetails = CartItems.ToList(),
                Total = Total,
            };

            // update order items in order
            if (OrderId != 0)
            {
                try {
                    bool isUpdateOrderItems = await _dao.UpdateOrderItems(order);
                    //if(_selectedDiscount != null)
                    //{
                    //    double? discountAmount = await _dao.ApplyDiscount(OrderId, int.Parse(_selectedDiscount.DiscountID));
                    //}
                    //if (_pointsToUse > 0)
                    //{
                    //    bool isApplyRewardPoint = await _dao.ApplyRewardPoint(OrderId, Subtotal - DiscountAmount, Customer.PhoneNumber, _pointsToUse);
                    //}
                    if (isUpdateOrderItems)
                    {
                        await MessageHelper.ShowSuccessMessage("Update order items successful", App.m_window.Content.XamlRoot);
                        return null;
                    } else
                    {
                        await MessageHelper.ShowErrorMessage("Fail when update order items", App.m_window.Content.XamlRoot);
                        return null;
                    }
                } catch {
                    return null;
                }
            }
            else
            {
                try
                {
                    OrderModel newOrder = await _dao.AddOrderAsync(order);
                    //if (_selectedDiscount != null)
                    //{
                    //    double? discountAmount = await _dao.ApplyDiscount(newOrder.OrderId, int.Parse(_selectedDiscount.DiscountID));
                    //}
                    if (newOrder == null)
                    {
                        await MessageHelper.ShowErrorMessage("Fail to create new order", App.m_window.Content.XamlRoot);
                        return null;
                    }
                    OrderId = newOrder.OrderId;
                    bool isUpdateTable = await _dao.UpdateTableAfterOrder(newOrder.OrderId, SelectedTableId);
                    if (!isUpdateTable)
                    {
                        await MessageHelper.ShowErrorMessage("Fail to create new", App.m_window.Content.XamlRoot);
                        return null;
                    }

                    newOrder.OrderDetails = order.OrderDetails;
                    OrderDataServices.Instance.Orders.Add(newOrder);
                    //CartItems.Clear();
                    //OnPropertyChanged(nameof(CartItems));
                    //OnPropertyChanged(nameof(Subtotal));
                    //OnPropertyChanged(nameof(Tax));
                    //OnPropertyChanged(nameof(Total));
                    await MessageHelper.ShowSuccessMessage("Create new order successful", App.m_window.Content.XamlRoot);
                    return new TableModel { tableId = SelectedTableId, isAvailable = false, currentOrderId = newOrder.OrderId };

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    await MessageHelper.ShowErrorMessage("Fail to create new order", App.m_window.Content.XamlRoot);
                    return null;
                }
            }
        }

        /// <summary>
        /// Checks out the current cart asynchronously.
        /// </summary>
        /// <returns>The table information after checkout.</returns>
        public async Task<TableModel> CheckOut()
        {
            var order = new OrderModel
            {
                OrderId = OrderId,
                //TableNumber = 1,
                //OrderTime = DateTime.Now,
                OrderDetails = CartItems.ToList(),
                Total = Total,
                Note = Note
            };
            try
            {
                if(OrderId == 0)
                {
                    OrderModel newOrder = await _dao.AddOrderAsync(order);
                    if (newOrder == null)
                    {
                        await MessageHelper.ShowErrorMessage("Fail to create new order", App.m_window.Content.XamlRoot);
                        return null;
                    }
                    OrderId = newOrder.OrderId;
                }
                if(Customer != null)
                {
                    await _dao.AddRewardPoints(Subtotal, Customer.CustomerID);
                }
                if (_selectedDiscount != null)
                {
                    double? discountAmount = await _dao.ApplyDiscount(OrderId, int.Parse(_selectedDiscount.DiscountID));
                }
                if (_pointsToUse > 0)
                {
                    bool isApplyRewardPoint = await _dao.ApplyRewardPoint(OrderId, Subtotal - DiscountAmount, Customer.PhoneNumber, _pointsToUse);
                }

                bool isCheckout = await _dao.CheckOut(SelectedTableId, OrderId, Note);
                if (!isCheckout)
                {
                    await MessageHelper.ShowErrorMessage("Fail to checkout", App.m_window.Content.XamlRoot);
                    return null;
                } else
                {
                    await MessageHelper.ShowSuccessMessage("Checkout successful", App.m_window.Content.XamlRoot);
                    CartItems.Clear();
                    OnPropertyChanged(nameof(CartItems));
                    Subtotal = 0;
                    OrderId = 0;
                    SelectedDiscount = null;
                    PointsToUse = 0;
                    OnPropertyChanged(nameof(SelectedDiscount));
                    OnPropertyChanged(nameof(Subtotal));
                    OnPropertyChanged(nameof(Tax));
                    OnPropertyChanged(nameof(Total));
                    OnPropertyChanged(nameof(Note));
                    return new TableModel { tableId = SelectedTableId, isAvailable = true, currentOrderId = null };
                }

            }
            catch (Exception e)
            {
                await MessageHelper.ShowErrorMessage("Fail to checkout", App.m_window.Content.XamlRoot);
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
