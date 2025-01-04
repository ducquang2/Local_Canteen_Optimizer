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
        //private ProductViewModel productViewModel;
        public Product()
        {
            this.InitializeComponent();
            //productViewModel = new ProductViewModel();
            //DataContext = productViewModel;

            // Khởi tạo danh sách sản phẩm
            productListControl = new ListProducts();
            productListControl.AddProductRequested += OnAddProductRequested;
            productListControl.EditProductRequested += OnEditProductRequested;

            // Khởi tạo form thêm sản phẩm
            addProductControl = new AddProduct();
            addProductControl.SaveRequested += OnAddSaveRequested;
            addProductControl.CancelRequested += OnCancelRequested;

            editProductControl = new EditProduct();
            editProductControl.SaveRequested += OnEditSaveRequested;
            editProductControl.CancelRequested += OnCancelRequested;

            // Hiển thị danh sách sản phẩm ban đầu
            ProductsContent.Content = productListControl;
        }

        private void OnAddProductRequested(object sender, EventArgs e)
        {
            // Khi nhấn nút Add Product, chuyển sang form thêm sản phẩm
            ProductsContent.Content = addProductControl;
        }
        private void OnEditProductRequested(object sender, FoodModel product)
        {
            editProductControl.SetProduct(product);
            ProductsContent.Content = editProductControl;
        }

        private void OnAddSaveRequested(object sender, FoodModel product)
        {
            // Khi lưu sản phẩm, thêm vào danh sách và quay lại danh sách sản phẩm
            productListControl.AddProduct(product);
            ProductsContent.Content = productListControl;
        }
        private void OnEditSaveRequested(object sender, FoodModel product)
        {
            productListControl.UpdateProduct(product);
            ProductsContent.Content = productListControl; // Quay lại danh sách sản phẩm
        }

        private void OnCancelRequested(object sender, EventArgs e)
        {
            // Khi hủy, quay lại danh sách sản phẩm
            ProductsContent.Content = productListControl;
        }
    }
}
