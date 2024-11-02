using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View
{
    public sealed partial class CartView : UserControl
    {
        public CartView()
        {
            this.InitializeComponent();
            //this.DataContext = new CartViewModel();
        }

        private void RemoveCartItemButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            CartItemModel itemToRemove = button?.Tag as CartItemModel;

            if (itemToRemove != null)
            {
                var cartViewModel = this.DataContext as CartViewModel;
                if (cartViewModel != null)
                {
                    // Gọi hàm xóa item trong CartViewModel
                    cartViewModel.RemoveItem(itemToRemove);
                }
            }
        }

        private void holdCartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartViewModel = this.DataContext as CartViewModel;
            if (cartViewModel != null)
            {
                // Gọi hàm xóa item trong CartViewModel
                cartViewModel.HoldCart();
            }
        }
    }
}
