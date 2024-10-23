using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Ultis
{
    public class CustomRadioButton : RadioButton
    {
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(CustomRadioButton), new PropertyMetadata(null));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }
    }
}
