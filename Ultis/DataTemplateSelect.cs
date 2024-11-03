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
            }

            //if (item is HomeViewModel)
            //{
            //    return HomeTemplate;
            //}
            //else if (item is CustomerViewModel)
            //{
            //    return CustomerTemplate;
            //}
            //else if (item is OrderViewModel)
            //{
            //    return OrderTemplate;
            //}
            //else if (item is ProductViewModel)
            //{
            //    return ProductTemplate;
            //}
            //else if (item is ReportViewModel)
            //{
            //    return ReportTemplate;
            //}
            //else if (item is SettingViewModel)
            //{
            //    return SettingTemplate;
            //}
            //else if (item is TableViewModel)
            //{
            //    return TableTemplate;
            //}
            //else if (item is TransactionViewModel)
            //{
            //    return TransactionTemplate;
            //}

            return base.SelectTemplateCore(item);
        }
    }
}
