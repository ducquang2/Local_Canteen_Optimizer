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
    public sealed partial class ListProducts : UserControl
    {
        ProductViewModel productViewModel;
        public event EventHandler AddProductRequested;
        public event EventHandler<FoodModel> EditProductRequested;
        public ListProducts()
        {
            this.InitializeComponent();
            InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            productViewModel = new ProductViewModel();
            await productViewModel.Init();
            UpdatePagingInfo_bootstrap();
        }

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

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            AddProductRequested?.Invoke(this, EventArgs.Empty);
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy sản phẩm từ nút Edit
            var product = (sender as Button).Tag as FoodModel;
            EditProductRequested?.Invoke(this, product);
        }   
        public void AddProduct(FoodModel product)
        {
            productViewModel.AddFoodItem(product);
        }
        public void UpdateProduct(FoodModel product)
        {
            productViewModel.UpdateProduct(product);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is FoodModel product)
            {
                productViewModel.DeleteProduct(product);
            }
        }

        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic item = pagesComboBox.SelectedItem;

            if (item != null)
            {
                productViewModel.Load(item.Page);
            }
        }


        private void keywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            handleSearchButtonClick();
        }

        public async Task handleSearchButtonClick()
        {
            await productViewModel.Load(1);
            UpdatePagingInfo_bootstrap();
        }
    }
}
