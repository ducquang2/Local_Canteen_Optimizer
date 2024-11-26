using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Model
{
    public class ApiOrderItem
    {
        public int order_item_id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public string product_name { get; set; }
        public string image_url { get; set; }
    }
}
