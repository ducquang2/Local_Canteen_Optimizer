using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Model
{
    public class DiscountModel
    {
        public string DiscountID { get; set; }
        public string DiscountName { get; set; }
        public string DiscountDescription { get; set; }
        public double DiscountValue { get; set; }
        public string DiscountType { get; set; }
        public string DiscountStatus { get; set; }
        public double DiscountMinOrderValue { get; set; }
        public double DiscountMaxValue { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public double DiscountAmount { get; set; }
    }
}
