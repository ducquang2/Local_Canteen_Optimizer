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
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View
{
    /// <summary>
    /// View for managing the cart in the Local Canteen Optimizer application.
    /// </summary>
    public sealed partial class CartView : UserControl
    {
        /// <summary>
        /// Event triggered when a request to add a table is made.
        /// </summary>
        public event EventHandler AddTableRequested;

        /// <summary>
        /// Event triggered when a request to hold the cart is made.
        /// </summary>
        public event EventHandler<TableModel> HoldCartRequested;

        /// <summary>
        /// Event triggered when a request to check out is made.
        /// </summary>
        public event EventHandler<TableModel> CheckOutRequested;
        public DiscountViewModel discountViewModel;
        public CustomerViewModel customerViewModel;

        /// <summary>
        /// ViewModel for managing discounts.
        /// </summary>
        public DiscountViewModel discountViewModel;

        /// <summary>
        /// ViewModel for managing customers.
        /// </summary>
        public CustomerViewModel customerViewModel;

        /// <summary>
        /// Initializes a new instance of the CartView class.
        /// </summary>
        public CartView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the click event for removing an item from the cart.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void RemoveCartItemButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FoodModel itemToRemove = button?.Tag as FoodModel;

            if (itemToRemove != null)
            {
                var cartViewModel = this.DataContext as CartViewModel;
                if (cartViewModel != null)
                {
                    cartViewModel.RemoveItem(itemToRemove);
                }
            }
        }

        /// <summary>
        /// Handles the click event for holding the cart.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private async void holdCartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartViewModel = this.DataContext as CartViewModel;
            if (cartViewModel != null)
            {
                TableModel table = await cartViewModel.HoldCart();
                HoldCartRequested?.Invoke(this, table);
            }
        }

        /// <summary>
        /// Handles the click event for selecting a table.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void SelectTable_Click(object sender, RoutedEventArgs e)
        {
            AddTableRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the click event for checking out the cart.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private async void checkoutCartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartViewModel = this.DataContext as CartViewModel;
            if (cartViewModel != null)
            {
                TableModel table = await cartViewModel.CheckOut();
                CheckOutRequested?.Invoke(this, table);
            }
        }
    }
}
