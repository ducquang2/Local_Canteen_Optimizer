using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Model
{
    public class TableModel
    {
        public int tableId { get; set; }
        public string tableName { get; set; }
        public bool isAvailable { get; set; }
        public int? currentOrderId { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}
