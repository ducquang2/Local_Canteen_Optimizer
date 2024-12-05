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

        public double Subtotal => CartItems.Sum(item => item.Price);
        public double Tax => Subtotal * 0;
        public double Total => Subtotal + Tax;
        public int OrderId { get; set; } = 1;

        private int selectedTableId;
        public int SelectedTableId
        {
            get => selectedTableId;
            set
            {
                selectedTableId = value;
                OnPropertyChanged(nameof(SelectedTableId));
                LoadProductsAsync();
            }
        }

        public CartViewModel()
        {
            _dao = new OrderDAOImp();
            selectedTableId = 0;
            CartItems = new ObservableCollection<FoodModel>();
            //RemoveItemCommand = new RelayCommand<FoodModel>(RemoveItem);
            //LoadProductsAsync();
        }

        public async Task LoadProductsAsync()
        {
            OrderModel orderModel = await _dao.GetOrderModelFromTable(selectedTableId);
            CartItems.Clear();
            if(orderModel == null)
            {
                OnPropertyChanged(nameof(CartItems));
                OnPropertyChanged(nameof(Subtotal));
                OnPropertyChanged(nameof(Tax));
                OnPropertyChanged(nameof(Total));
                return;
            }
            foreach (var item in orderModel.OrderDetails)
            {
                CartItems.Add(item);
            }
            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }

        public void AddItemToCart(FoodModel item)
        {
            CartItems.Add(item);
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }

        public void RemoveItem(FoodModel item)
        {
            CartItems.Remove(item);
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }

        public async Task<TableModel> HoldCart()
        {
           
            var order = new OrderModel
            {
                //OrderId = OrderDataServices.OrderId.ToString(),
                //TableNumber = 1,
                //OrderTime = DateTime.Now,
                OrderDetails = CartItems.ToList(),
                Total = Total
            };
            //OrderDataServices.OrderId++;
            try
            {
                OrderModel newOrder = await _dao.AddOrderAsync(order);
                if (newOrder == null)
                {
                    await MessageHelper.ShowSuccessMessage("Fail to create new order", App.m_window.Content.XamlRoot);
                    return null;
                }
                bool isUpdateTable = await _dao.UpdateTableAfterOrder(newOrder.OrderId, SelectedTableId);
                if (!isUpdateTable)
                {
                    await MessageHelper.ShowSuccessMessage("Fail to update table", App.m_window.Content.XamlRoot);
                    return null;
                }


                newOrder.OrderDetails = order.OrderDetails;
                OrderDataServices.Instance.Orders.Add(newOrder);
                CartItems.Clear();
                OnPropertyChanged(nameof(CartItems));
                OnPropertyChanged(nameof(Subtotal));
                OnPropertyChanged(nameof(Tax));
                OnPropertyChanged(nameof(Total));
                await MessageHelper.ShowSuccessMessage("Create new order successful", App.m_window.Content.XamlRoot);
                return new TableModel { tableId = SelectedTableId, isAvailable = false, currentOrderId = newOrder.OrderId };

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await MessageHelper.ShowSuccessMessage("Fail to create new order", App.m_window.Content.XamlRoot);
                return null;
            }
        }

        public async Task<TableModel> CheckOut()
        {
            try
            {
                bool isCheckout = await _dao.CheckOut(SelectedTableId);
                if (!isCheckout)
                {
                    await MessageHelper.ShowSuccessMessage("Fail to checkout", App.m_window.Content.XamlRoot);
                    return null;
                } else
                {
                    await MessageHelper.ShowSuccessMessage("Checkout successful", App.m_window.Content.XamlRoot);
                    CartItems.Clear();
                    OnPropertyChanged(nameof(CartItems));
                    OnPropertyChanged(nameof(Subtotal));
                    OnPropertyChanged(nameof(Tax));
                    OnPropertyChanged(nameof(Total));
                    return new TableModel { tableId = SelectedTableId, isAvailable = true, currentOrderId = null };
                }

            }
            catch (Exception e)
            {
                await MessageHelper.ShowSuccessMessage("Fail to checkout", App.m_window.Content.XamlRoot);
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
