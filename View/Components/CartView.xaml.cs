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

        /// <summary>
        /// Handles the click event for applying a discount to the cart.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private async void DiscountButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cartViewModel = this.DataContext as CartViewModel;
                double totalPrice = cartViewModel?.Subtotal ?? 0;

                discountViewModel = new DiscountViewModel();
                List<DiscountModel> discounts = await discountViewModel.getEligibleDiscount(totalPrice);
                var selectedDiscount = await ShowDiscountsDialog("Discount", discounts, App.m_window.Content.XamlRoot);

                if (selectedDiscount != null)
                {
                    if (cartViewModel != null)
                    {
                        cartViewModel.SelectedDiscount = selectedDiscount;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        /// <summary>
        /// Displays a dialog for selecting a discount.
        /// </summary>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="discounts">The list of available discounts.</param>
        /// <param name="xamlRoot">The XAML root for the dialog.</param>
        /// <returns>The selected discount, or null if no discount was selected.</returns>
        public async Task<DiscountModel> ShowDiscountsDialog(string title, List<DiscountModel> discounts, Microsoft.UI.Xaml.XamlRoot xamlRoot)
        {
            if (xamlRoot == null)
                throw new ArgumentNullException(nameof(xamlRoot), "XamlRoot cannot be null");

            var listView = new ListView
            {
                ItemsSource = discounts,
                ItemTemplate = (DataTemplate)this.Resources["DiscountTemplate"],
                SelectionMode = ListViewSelectionMode.Single
            };

            var dialog = new ContentDialog
            {
                Title = title,
                Content = listView,
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Ok",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = xamlRoot
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary && listView.SelectedItem is DiscountModel selectedDiscount)
            {
                return selectedDiscount;
            }
            return null;
        }

        /// <summary>
        /// Handles the click event for searching a customer by phone number.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private async void OnSearchCustomerClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                String phoneNumber = PhoneNumberTextBox.Text;
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    return;
                }
                var cartViewModel = this.DataContext as CartViewModel;

                customerViewModel = new CustomerViewModel();
                CustomerModel customer = await customerViewModel.getCustomerByPhone(phoneNumber);

                if (cartViewModel != null)
                {
                    cartViewModel.Customer = customer;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        /// <summary>
        /// Handles the click event for using reward points.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void OnUsePointsClicked(object sender, RoutedEventArgs e)
        {
            if (DataContext is CartViewModel viewModel)
            {
                if (int.TryParse(PointToUseTextBox.Text, out int pointsToUse))
                {
                    if (pointsToUse > 0 && pointsToUse <= viewModel.Customer.RewardPoints)
                    {
                        viewModel.PointsToUse = pointsToUse;
                    }
                    else
                    {
                        viewModel.PointsToUse = 0;
                    }
                }
            }
        }
    }
}
