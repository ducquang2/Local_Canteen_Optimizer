using Local_Canteen_Optimizer.Commands;
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
    public class CartViewModel : INotifyPropertyChanged
    {
        private IOrderDAO _dao = null;
        public ObservableCollection<FoodModel> CartItems { get; set; }
        public ICommand RemoveItemCommand { get; set; }

        public double Subtotal { get; set; } = 0;
        public double Tax => Subtotal * 0;
        public double Total => Subtotal + Tax;
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

        public CartViewModel()
        {
            _dao = new OrderDAOImp();
            selectedTableId = 1;
            CartItems = new ObservableCollection<FoodModel>();
            //RemoveItemCommand = new RelayCommand<FoodModel>(RemoveItem);
            LoadOrderItemsAsync();
        }

        public async Task LoadOrderItemsAsync()
        {
            OrderModel orderModel = await _dao.GetOrderModelFromTable(selectedTableId);
            CartItems.Clear();
            if(orderModel == null)
            {
                OrderId = 0;
                OnPropertyChanged(nameof(CartItems));
                Subtotal = 0;
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
            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(Note));
        }

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

        public void RemoveItem(FoodModel item)
        {
            CartItems.Remove(item);
            Subtotal -= item.Price * item.QuantityBuy;
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }

        public async Task<TableModel> HoldCart()
        {
            var order = new OrderModel
            {
                OrderId = OrderId,
                //TableNumber = 1,
                //OrderTime = DateTime.Now,
                OrderDetails = CartItems.ToList(),
                Total = Total
            };

            // update order items in order
            if (OrderId != 0)
            {
                try {
                    bool isUpdateOrderItems = await _dao.UpdateOrderItems(order);
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
                    if (newOrder == null)
                    {
                        await MessageHelper.ShowErrorMessage("Fail to create new order", App.m_window.Content.XamlRoot);
                        return null;
                    }
                    bool isUpdateTable = await _dao.UpdateTableAfterOrder(newOrder.OrderId, SelectedTableId);
                    if (!isUpdateTable)
                    {
                        await MessageHelper.ShowErrorMessage("Fail to update table", App.m_window.Content.XamlRoot);
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
