using Local_Canteen_Optimizer.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Ultis
{
    public class DataTemplateSelect : DataTemplateSelector
    {
        public DataTemplate HomeTemplate { get; set; }
        public DataTemplate CustomerTemplate { get; set; }
        public DataTemplate OrderTemplate { get; set; }
        public DataTemplate ProductTemplate { get; set; }
        public DataTemplate ReportTemplate { get; set; }
        public DataTemplate SettingTemplate { get; set; }
        public DataTemplate TableTemplate { get; set; }
        public DataTemplate TransactionTemplate { get; set; }
        public DataTemplate ManageUserTemplate { get; set; }
        public DataTemplate DiscountTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            switch (item)
            {
                case HomeViewModel: return HomeTemplate;
                case CustomerViewModel: return CustomerTemplate;
                case OrderViewModel: return OrderTemplate;
                case ProductViewModel: return ProductTemplate;
                case ReportViewModel: return ReportTemplate;
                case SettingViewModel: return SettingTemplate;
                case TableViewModel: return TableTemplate;
                case TransactionViewModel: return TransactionTemplate;
                case ManageUserViewModel: return ManageUserTemplate;
                case DiscountViewModel: return DiscountTemplate;
            }

            return base.SelectTemplateCore(item);
        }
    }
}
