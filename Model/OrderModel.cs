using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Model
{
    public class OrderModel
    {
        public int TableNumber { get; set; }
        public string OrderId { get; set; }
        public DateTime OrderTime { get; set; }
        public double Total { get; set; }
        public List<CartItemModel> OrderDetails { get; set; }
    }
}
