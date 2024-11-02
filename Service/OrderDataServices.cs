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
        private static OrderDataServices _instance;
        public static OrderDataServices Instance => _instance ??= new OrderDataServices();

        public ObservableCollection<OrderModel> Orders { get; }

        private OrderDataServices()
        {
            Orders = new ObservableCollection<OrderModel>();
        }
    }
}
