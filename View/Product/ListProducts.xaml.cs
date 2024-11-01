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
    public sealed partial class ListProducts : UserControl
    {
        ProductViewModel productViewModel;
        public event EventHandler AddProductRequested;
        public event EventHandler<FoodModel> EditProductRequested;
        public ListProducts()
        {
            this.InitializeComponent();
            productViewModel = new ProductViewModel();
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
    }
}
