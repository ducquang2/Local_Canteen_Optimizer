using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.View.Cashier;
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

namespace Local_Canteen_Optimizer.View.SellProduct
{
    public sealed partial class SellProduct : UserControl
    {
        private Home homeControl;
        private Table tableControl;
        public SellProduct()
        {
            this.InitializeComponent();

            // Khởi tạo trang bán hàng
            homeControl = new Home();
            homeControl.CartViewControl.AddTableRequested += OnAddTableRequested;
            homeControl.CartViewControl.HoldCartRequested += OnHoldCartRequested;
            homeControl.CartViewControl.CheckOutRequested += OnCheckOutRequested;

            // Khởi tạo trang thêm bàn
            tableControl = new Table();
            tableControl.SaveTableRequested += OnSaveTableRequested;
            tableControl.CancelRequested += OnCancelRequested;

            // Hiển thị trang bán hàng
            SellProductContent.Content = homeControl;
        }

        private void OnAddTableRequested(object sender, EventArgs e)
        {
            // Khi nhấn nút Add Product, chuyển sang màn hình chọn bàn
            SellProductContent.Content = tableControl;
        }

        private void OnSaveTableRequested(object sender, int tableId)
        {
            homeControl.CartViewModel.SelectedTableId = tableId;
            SellProductContent.Content = homeControl;
        }

        private void OnHoldCartRequested(object sender, TableModel table)
        {
            tableControl.tableViewModel.updateTable(table);
        }

        private void OnCheckOutRequested(object sender, TableModel table)
        {
            tableControl.tableViewModel.updateTable(table);
        }

        private void OnCancelRequested(object sender, EventArgs e)
        {
            // Khi hủy, quay lại danh sách sản phẩm
            SellProductContent.Content = homeControl;
        }

    }
}
