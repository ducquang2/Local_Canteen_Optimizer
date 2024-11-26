using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Vml;
using Local_Canteen_Optimizer.DAO.OrderDAO;
using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Service
{
    class OrderDataServices
    {
        private IOrderDAO _dao = null;
        private static OrderDataServices _instance;
        public static int OrderId { get; set; } = 1;
        public static OrderDataServices Instance => _instance ??= new OrderDataServices();

        public ObservableCollection<OrderModel> Orders { get; }

        private OrderDataServices()
        {
            _dao = new OrderDAOImp();
            Orders = new ObservableCollection<OrderModel>();
            LoadProductsAsync();
        }

        public async Task LoadProductsAsync()
        {
            List<OrderModel> listOrder = await _dao.GetAllOrders();
            foreach (var item in listOrder)
            {
               Orders.Add(item);
            }
        }
    }
}
