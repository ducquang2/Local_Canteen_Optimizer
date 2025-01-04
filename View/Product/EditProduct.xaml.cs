using Local_Canteen_Optimizer.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View.Product
{
    /// <summary>
    /// UserControl for editing a product.
    /// </summary>
    public sealed partial class EditProduct : UserControl
    {
        /// <summary>
        /// Event triggered when the save button is clicked.
        /// </summary>
        public event EventHandler<FoodModel> SaveRequested;

        /// <summary>
        /// Event triggered when the cancel button is clicked.
        /// </summary>
        public event EventHandler CancelRequested;

        private FoodModel currentProduct;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditProduct"/> class.
        /// </summary>
        public EditProduct()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Sets the product to be edited.
        /// </summary>
        /// <param name="product">The product to be edited.</param>
        public void SetProduct(FoodModel product)
        {
            currentProduct = product;
            IdTextBox.Text = product.ProductID;
            NameTextBox.Text = product.Name;
            ImageTextBox.Text = product.ImageSource;
            PriceTextBox.Text = product.Price.ToString();
            QuantityTextBox.Text = product.Quantity.ToString();
        }

        /// <summary>
        /// Handles the click event of the save button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;

            // Reset error messages
            NameErrorText.Visibility = Visibility.Collapsed;
            ImageErrorText.Visibility = Visibility.Collapsed;
            PriceErrorText.Visibility = Visibility.Collapsed;
            QuantityErrorText.Visibility = Visibility.Collapsed;

            // Validate Name
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                NameErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // Validate Image Source
            if (string.IsNullOrWhiteSpace(ImageTextBox.Text))
            {
                ImageErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // Validate Price
            if (!double.TryParse(PriceTextBox.Text, out var price) || price < 0)
            {
                PriceErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // Validate Quantity
            if (!int.TryParse(QuantityTextBox.Text, out var quantity) || quantity < 0)
            {
                QuantityErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // If there are errors, stop here
            if (hasError) return;

            currentProduct.ProductID = IdTextBox.Text;
            currentProduct.Name = NameTextBox.Text;
            currentProduct.ImageSource = ImageTextBox.Text;
            currentProduct.Price = price;
            currentProduct.Quantity = quantity;

            SaveRequested?.Invoke(this, currentProduct);
        }

        /// <summary>
        /// Handles the click event of the cancel button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
