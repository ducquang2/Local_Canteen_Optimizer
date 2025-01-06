using Local_Canteen_Optimizer.Helper;
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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View.Product
{
    /// <summary>
    /// UserControl for listing products.
    /// </summary>
    public sealed partial class ListProducts : UserControl
    {
        /// <summary>
        /// ViewModel for managing products.
        /// </summary>
        ProductViewModel productViewModel;

        /// <summary>
        /// Event triggered when a request to add a product is made.
        /// </summary>
        public event EventHandler AddProductRequested;

        /// <summary>
        /// Event triggered when a request to edit a product is made.
        /// </summary>
        public event EventHandler<FoodModel> EditProductRequested;

        /// <summary>
        /// Initializes a new instance of the ListProducts class.
        /// </summary>
        public ListProducts()
        {
            this.InitializeComponent();
            InitializeAsync();
        }

        /// <summary>
        /// Asynchronously initializes the ViewModel and updates paging information.
        /// </summary>
        public async Task InitializeAsync()
        {
            productViewModel = new ProductViewModel();
            await productViewModel.Init();
            UpdatePagingInfo_bootstrap();
        }

        /// <summary>
        /// Updates the paging information for the product list.
        /// </summary>
        void UpdatePagingInfo_bootstrap()
        {
            var infoList = new List<object>();
            for (int i = 1; i <= productViewModel.TotalPages; i++)
            {
                infoList.Add(new
                {
                    Page = i,
                    Total = productViewModel.TotalPages
                });
            };

            pagesComboBox.ItemsSource = infoList;
            pagesComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the Add Product button click event.
        /// </summary>
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            AddProductRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the Edit button click event.
        /// </summary>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var product = (sender as Button).Tag as FoodModel;
            EditProductRequested?.Invoke(this, product);
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="product">The product to add.</param>
        public async void AddProduct(FoodModel product)
        {
            try
            {
                await productViewModel.AddFoodItem(product);
                await MessageHelper.ShowSuccessMessage("Add new product successful", App.m_window.Content.XamlRoot);
            }
            catch (Exception e)
            {
                await MessageHelper.ShowErrorMessage("Fail to add new product", App.m_window.Content.XamlRoot);
            }
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="product">The product to update.</param>
        public async void UpdateProduct(FoodModel product)
        {
            await productViewModel.UpdateProduct(product);
        }

        /// <summary>
        /// Handles the Remove button click event.
        /// </summary>
        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is FoodModel product)
            {
                await productViewModel.DeleteProduct(product);
            }
        }

        /// <summary>
        /// Handles the page selection change event.
        /// </summary>
        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic item = pagesComboBox.SelectedItem;

            if (item != null)
            {
                productViewModel.Load(item.Page);
            }
        }

        /// <summary>
        /// Handles the sort order selection change event.
        /// </summary>
        public void SortOrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var selectedItem = comboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    bool isAscending = bool.Parse(selectedItem.Tag.ToString());
                    productViewModel.LoadProductSort(isAscending);
                }
            }
        }

        /// <summary>
        /// Handles the keyword text box text change event.
        /// </summary>
        private void keywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        /// <summary>
        /// Handles the search button click event.
        /// </summary>
        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            await handleSearchButtonClick();
        }

        /// <summary>
        /// Handles the search button click event asynchronously.
        /// </summary>
        public async Task handleSearchButtonClick()
        {
            await productViewModel.Load(1);
            UpdatePagingInfo_bootstrap();
        }

        /// <summary>
        /// Handles the import product from Excel button click event.
        /// </summary>
        private async void ImportProductExcelButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = await productViewModel.PickExcelFileAsync();
            if (!string.IsNullOrEmpty(filePath))
            {
                await productViewModel.ImportProductsFromExcel(filePath);
            }
        }

        /// <summary>
        /// Handles the export product to Excel button click event.
        /// </summary>
        private async void ExportProductExcelButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFilePath = await productViewModel.PickSaveFileAsync();
            if (string.IsNullOrEmpty(saveFilePath))
            {
                return;
            }
            try
            {
                await productViewModel.LoadAllProductsAsync();
                await productViewModel.ExportToExcel(saveFilePath, productViewModel.allFoodItems);
            }
            catch
            {
                await MessageHelper.ShowErrorMessage("Fail to export", App.m_window.Content.XamlRoot);
            }
        }
    }
}
