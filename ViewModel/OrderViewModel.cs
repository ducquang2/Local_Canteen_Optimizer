using Local_Canteen_Optimizer.DAO.OrderDAO;
using Local_Canteen_Optimizer.DAO.ProductDAO;
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

namespace Local_Canteen_Optimizer.ViewModel
{
    /// <summary>
    /// ViewModel class for managing orders.
    /// </summary>
    public class OrderViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Data access object for orders.
        /// </summary>
        private IOrderDAO _dao = null;

        /// <summary>
        /// Collection of orders.
        /// </summary>
        public ObservableCollection<OrderModel> Orders => OrderDataServices.Instance.Orders;

        /// <summary>
        /// Total number of pages.
        /// </summary>
        public int TotalPages => OrderDataServices.Instance.TotalPages;

        /// <summary>
        /// Current order being managed.
        /// </summary>
        public OrderModel currentOrder;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderViewModel"/> class.
        /// </summary>
        //public OrderViewModel()
        //{
        //    _dao = new OrderDAOImp();
        //}

        /// <summary>
        /// Initializes the ViewModel.
        /// </summary>
        public async Task Init()
        {
            _dao = new OrderDAOImp();
            try
            {
                await OrderDataServices.Instance.LoadOrdersAsync();
            }
            catch
            {
                await MessageHelper.ShowErrorMessage("Can't get any orders", App.m_window.Content.XamlRoot);
            }
        }

        /// <summary>
        /// Updates the current order model with the specified order.
        /// </summary>
        /// <param name="order">The order to update.</param>
        public async Task UpdateOrderModel(OrderModel order)
        {
            currentOrder = order;
            List<FoodModel> cartItemModels = await _dao.GetAllOrderItems(order.OrderId);
            currentOrder.OrderDetails = cartItemModels;
            OnPropertyChanged(nameof(currentOrder));
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a property value changes.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
