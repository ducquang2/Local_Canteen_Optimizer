using Local_Canteen_Optimizer.DAO.OrderDAO;
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
    public class OrderViewModel : INotifyPropertyChanged
    {
        private IOrderDAO _dao = null;
        public ObservableCollection<OrderModel> Orders => OrderDataServices.Instance.Orders;
        public OrderModel currentOrder;

        public OrderViewModel()
        {
            _dao = new OrderDAOImp();
        }

        public async Task UpdateOrderModel(OrderModel order)
        {
            currentOrder = order;
            List<CartItemModel> cartItemModels = await _dao.GetAllOrderItems(order.OrderId);
            currentOrder.OrderDetails = cartItemModels;
            OnPropertyChanged(nameof(currentOrder));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
