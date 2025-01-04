using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Vml;
using Local_Canteen_Optimizer.DAO.OrderDAO;
using Local_Canteen_Optimizer.Helper;
using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Service
{
    /// <summary>
    /// Service class for managing order data.
    /// </summary>
    class OrderDataServices
    {
        private IOrderDAO _dao = null;
        private static OrderDataServices _instance;

        /// <summary>
        /// Gets or sets a value indicating whether the date is sorted in ascending order.
        /// </summary>
        public bool DateAscending { get; set; } = true;

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of rows per page.
        /// </summary>
        public int RowsPerPage { get; set; } = 10;

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; } = 0;

        /// <summary>
        /// Gets or sets the total number of items.
        /// </summary>
        public int TotalItems { get; set; } = 0;

        /// <summary>
        /// Gets the singleton instance of the OrderDataServices class.
        /// </summary>
        public static OrderDataServices Instance => _instance ??= new OrderDataServices();

        /// <summary>
        /// Gets the collection of orders.
        /// </summary>
        public ObservableCollection<OrderModel> Orders { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderDataServices"/> class.
        /// </summary>
        private OrderDataServices()
        {
            _dao = new OrderDAOImp();
            Orders = new ObservableCollection<OrderModel>();
        }

        /// <summary>
        /// Loads the orders for the specified page.
        /// </summary>
        /// <param name="page">The page number to load.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task Load(int page)
        {
            CurrentPage = page;
            await LoadOrdersAsync();
        }

        /// <summary>
        /// Loads the orders sorted by date.
        /// </summary>
        /// <param name="dateAscending">if set to <c>true</c> the orders are sorted in ascending order; otherwise, descending.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task LoadOrderSort(bool dateAscending)
        {
            DateAscending = dateAscending;
            await LoadOrdersAsync();
        }

        /// <summary>
        /// Loads the orders asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task LoadOrdersAsync()
        {
            var (totalItems, listOrder) = await _dao.GetAllOrders(CurrentPage, RowsPerPage, DateAscending);
            if (totalItems == 0)
            {
                await MessageHelper.ShowErrorMessage("Can't get any order", App.m_window.Content.XamlRoot);
                return;
            }
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
