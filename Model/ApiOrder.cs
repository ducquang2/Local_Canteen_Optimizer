using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Model
{
    public class ApiOrder
    {
        public int order_id { get; set; }
        public string order_status { get; set; }
        public double total_price { get; set; }
        public string discount_price { get; set; }
        public int reward_value_used { get; set; }
        public string final_price { get; set; }

        public DateTime created_at { get; set; }
        public string note { get; set; }
    }
}
