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
    /// <summary>
    /// Converter class to convert boolean values to SolidColorBrush.
    /// </summary>
    class BooleanToColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a SolidColorBrush.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The target type of the conversion.</param>
        /// <param name="parameter">An optional parameter for the conversion.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>A SolidColorBrush based on the boolean value.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isAvailable)
            {
                return isAvailable ? new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)) : new SolidColorBrush(Windows.UI.Color.FromArgb(255, 252, 128, 25));
            }
            return new SolidColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
        }

        /// <summary>
        /// Converts a SolidColorBrush back to a boolean value.
        /// </summary>
        /// <param name="value">The SolidColorBrush to convert back.</param>
        /// <param name="targetType">The target type of the conversion.</param>
        /// <param name="parameter">An optional parameter for the conversion.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>A boolean value based on the SolidColorBrush.</returns>
        /// <exception cref="NotImplementedException">Thrown when the method is not implemented.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
