using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Helper
{
    public class OrderStatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string status)
            {
                return status.ToLower() switch
                {
                    "confirmed" => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 9, 170, 41)),
                    "pending" => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 252, 128, 25)),
                    _ => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)),
                };
            }

            return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
