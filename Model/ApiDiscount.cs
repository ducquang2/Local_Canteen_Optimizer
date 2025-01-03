using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Model
{
    public class ApiDiscount
    {
        public double promotion_id { get; set; }
        public string promotion_name { get; set; }
        public string description { get; set; }
        public string discount_type { get; set; }
        public string discount_value { get; set; }
        public double min_order_value { get; set; }
        public double max_discount_amount { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public bool is_active { get; set; }
        public string discount_amount { get; set; }
    }
}
