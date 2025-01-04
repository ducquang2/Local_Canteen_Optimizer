using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Helper
{
    class BooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isAvailable)
            {
                return isAvailable ? new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)): new SolidColorBrush(Windows.UI.Color.FromArgb(255,252,128,25));
            }
            return new SolidColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
