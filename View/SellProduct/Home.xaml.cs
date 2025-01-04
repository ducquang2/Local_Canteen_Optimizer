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
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View.Cashier
{
    public sealed partial class Home : UserControl
    {
        /// <summary>
        /// ViewModel for the cart.
        /// </summary>
        CartViewModel cartViewModel;

        /// <summary>
        /// ViewModel for the home view.
        /// </summary>
        HomeViewModel homeViewModel;

        /// <summary>
        /// Gets the CartView control.
        /// </summary>
        public CartView CartViewControl => CartView;

        /// <summary>
        /// Gets the CartViewModel.
        /// </summary>
        public CartViewModel CartViewModel => cartViewModel;

        /// <summary>
        /// Initializes a new instance of the Home class.
        /// </summary>
        public Home()
        {
            this.InitializeComponent();
            cartViewModel = new CartViewModel();
            homeViewModel = new HomeViewModel(cartViewModel);
            this.DataContext = homeViewModel;
            this.CartView.DataContext = cartViewModel;
        }

        /// <summary>
        /// Handles the click event for a food item.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void FoodItem_Click(object sender, ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// Handles the click event for the Add to Cart button.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void AddToCartButton(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            // Lấy thông tin món ăn từ Tag của Button
            FoodModel selectedFoodItem = button?.Tag as FoodModel;

            if (selectedFoodItem != null)
            {
                // Thêm món ăn vào giỏ hàng
                cartViewModel.AddItemToCart(selectedFoodItem);
            }
        }

        /// <summary>
        /// Handles the click event for the search button.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            await handleSearchButtonClick();
        }

        /// <summary>
        /// Handles the search button click event asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task handleSearchButtonClick()
        {
            await homeViewModel.searchProductsAsync();
        }

        /// <summary>
        /// Handles the click event for the filter button.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private async void filterButton_Click(object sender, RoutedEventArgs e)
        {
            await handleFilterButtonClick();
        }

        /// <summary>
        /// Handles the filter button click event asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task handleFilterButtonClick()
        {
            await homeViewModel.filterProductsAsync();
        }
    }
}
