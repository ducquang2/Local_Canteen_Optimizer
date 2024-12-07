﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Model
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public DateTime OrderTime { get; set; }
        public double Total { get; set; }
        public string OrderStatus { get; set; }
        public List<FoodModel> OrderDetails { get; set; }
    }
}
