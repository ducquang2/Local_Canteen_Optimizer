using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Model
{
    public class FoodModel
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string ImageSource { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
