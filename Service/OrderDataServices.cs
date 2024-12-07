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
        public bool DateAscending { get; set; } = true;
        public int CurrentPage { get; set; } = 1;
        public int RowsPerPage { get; set; } = 10;
        public int TotalPages { get; set; } = 0;
        public int TotalItems { get; set; } = 0;
        public static OrderDataServices Instance => _instance ??= new OrderDataServices();

        public ObservableCollection<OrderModel> Orders { get; }

        private OrderDataServices()
        {
            _dao = new OrderDAOImp();
            Orders = new ObservableCollection<OrderModel>();
            //LoadProductsAsync();
        }

        public async Task Load(int page)
        {
            CurrentPage = page;
            await LoadOrdersAsync();
        }

        public async Task LoadOrderSort(bool dateAscending)
        {
            DateAscending = dateAscending;
            await LoadOrdersAsync();
        }

        public async Task LoadOrdersAsync()
        {
            var (totalItems, listOrder) = await _dao.GetAllOrders(CurrentPage, RowsPerPage, DateAscending);
            Orders.Clear();
            foreach (var item in listOrder)
            {
               Orders.Add(item);
            }

            TotalItems = totalItems;
            TotalPages = (TotalItems / RowsPerPage) + ((TotalItems % RowsPerPage == 0) ? 0 : 1);
        }
    }
}
