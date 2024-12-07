using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Model
{
    public class ApiSeats
    {
        public int table_id { get; set; }
        public string table_name { get; set; }
        public bool is_available { get; set; }
        public int? current_order_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
