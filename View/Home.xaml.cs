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
using Local_Canteen_Optimizer.ViewModel;
using Local_Canteen_Optimizer.Model;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View
{
    public sealed partial class Home : UserControl
    {
        CartViewModel cartViewModel;
        HomeViewModel homeViewModel;
        public Home()
        {
            this.InitializeComponent();
            cartViewModel = new CartViewModel();
            homeViewModel = new HomeViewModel(cartViewModel);
            this.DataContext = homeViewModel;
            this.CartView.DataContext = cartViewModel;
        }

        private void FoodItem_Click(object sender, ItemClickEventArgs e)
        {

        }

        private void AddToCartButton(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            // Lấy thông tin món ăn từ Tag của Button
            FoodModel selectedFoodItem = button?.Tag as FoodModel;

            if (selectedFoodItem != null)
            {
                // Tạo CartItemModel từ FoodItemModel
                var cartItem = new CartItemModel
                {
                    Id = selectedFoodItem.ProductID,
                    Name = selectedFoodItem.Name,
                    Price = selectedFoodItem.Price
                };

                // Thêm món ăn vào giỏ hàng
                cartViewModel.AddItemToCart(cartItem);
            }
        }
    }
}
