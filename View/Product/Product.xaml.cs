using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View.Product
{
    public sealed partial class Product : UserControl
    {
        private ListProducts productListControl;
        private AddProduct addProductControl;
        private EditProduct editProductControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        public Product()
        {
            this.InitializeComponent();

            // Initialize product list control
            productListControl = new ListProducts();
            productListControl.AddProductRequested += OnAddProductRequested;
            productListControl.EditProductRequested += OnEditProductRequested;

            // Initialize add product control
            addProductControl = new AddProduct();
            addProductControl.SaveRequested += OnAddSaveRequested;
            addProductControl.CancelRequested += OnCancelRequested;

            // Initialize edit product control
            editProductControl = new EditProduct();
            editProductControl.SaveRequested += OnEditSaveRequested;
            editProductControl.CancelRequested += OnCancelRequested;

            // Display initial product list
            ProductsContent.Content = productListControl;
        }

        /// <summary>
        /// Handles the AddProductRequested event of the productListControl control.
        /// Switches to the add product form.
        /// </summary>
        private void OnAddProductRequested(object sender, EventArgs e)
        {
            ProductsContent.Content = addProductControl;
        }

        /// <summary>
        /// Handles the EditProductRequested event of the productListControl control.
        /// Switches to the edit product form.
        /// </summary>
        private void OnEditProductRequested(object sender, FoodModel product)
        {
            editProductControl.SetProduct(product);
            ProductsContent.Content = editProductControl;
        }

        /// <summary>
        /// Handles the SaveRequested event of the addProductControl control.
        /// Adds the product to the list and switches back to the product list.
        /// </summary>
        private void OnAddSaveRequested(object sender, FoodModel product)
        {
            productListControl.AddProduct(product);
            ProductsContent.Content = productListControl;
        }

        /// <summary>
        /// Handles the SaveRequested event of the editProductControl control.
        /// Updates the product in the list and switches back to the product list.
        /// </summary>
        private void OnEditSaveRequested(object sender, FoodModel product)
        {
            productListControl.UpdateProduct(product);
            ProductsContent.Content = productListControl;
        }

        /// <summary>
        /// Handles the CancelRequested event of the addProductControl and editProductControl controls.
        /// Switches back to the product list.
        /// </summary>
        private void OnCancelRequested(object sender, EventArgs e)
        {
            ProductsContent.Content = productListControl;
        }
    }
}
