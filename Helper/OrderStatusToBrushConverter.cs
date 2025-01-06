using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Helper
{
    /// <summary>
    /// Converts order status strings to SolidColorBrush objects.
    /// </summary>
    public class OrderStatusToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts an order status string to a SolidColorBrush.
        /// </summary>
        /// <param name="value">The order status string.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="language">The language (not used).</param>
        /// <returns>A SolidColorBrush corresponding to the order status.</returns>
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

        /// <summary>
        /// Not implemented. Throws a NotImplementedException.
        /// </summary>
        /// <param name="value">The value to convert back (not used).</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="language">The language (not used).</param>
        /// <returns>Throws a NotImplementedException.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
