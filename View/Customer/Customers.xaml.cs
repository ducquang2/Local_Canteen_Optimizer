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
using Local_Canteen_Optimizer.View.Product;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View.Customer
{
    public sealed partial class Customers : UserControl
    {
        private ListCustomer customerListControl;
        private AddCustomer addCustomerControl;
        private EditCustomer editCustomerControl;
        public Customers()
        {
            this.InitializeComponent();
            //productViewModel = new ProductViewModel();
            //DataContext = productViewModel;

            // Khởi tạo danh sách sản phẩm
            customerListControl = new ListCustomer();
            customerListControl.AddCustomerRequested += OnAddCustomerRequested;
            customerListControl.EditCustomerRequested += OnEditCustomerRequested;

            // Khởi tạo form thêm sản phẩm
            addCustomerControl = new AddCustomer();
            addCustomerControl.SaveRequested += OnAddSaveRequested;
            addCustomerControl.CancelRequested += OnCancelRequested;

            editCustomerControl = new EditCustomer();
            editCustomerControl.SaveRequested += OnEditSaveRequested;
            editCustomerControl.CancelRequested += OnCancelRequested;

            // Hiển thị danh sách sản phẩm ban đầu
            CustomersContent.Content = customerListControl;
        }

        private void OnAddCustomerRequested(object sender, EventArgs e)
        {
            // Khi nhấn nút Add Customer, chuyển sang form thêm sản phẩm
            CustomersContent.Content = addCustomerControl;
        }
        private void OnEditCustomerRequested(object sender, CustomerModel customer)
        {
            editCustomerControl.SetCustomer(customer);
            CustomersContent.Content = editCustomerControl;
        }

        private void OnAddSaveRequested(object sender, CustomerModel customer)
        {
            // Khi lưu sản phẩm, thêm vào danh sách và quay lại danh sách sản phẩm
            customerListControl.AddCustomer(customer);
            CustomersContent.Content = customerListControl;
        }
        private void OnEditSaveRequested(object sender, CustomerModel customer)
        {
            customerListControl.UpdateCustomer(customer);
            CustomersContent.Content = customerListControl; // Quay lại danh sách sản phẩm
        }

        private void OnCancelRequested(object sender, EventArgs e)
        {
            // Khi hủy, quay lại danh sách sản phẩm
            CustomersContent.Content = customerListControl;
        }
    }
}
