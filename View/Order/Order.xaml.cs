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
using Local_Canteen_Optimizer.View.Product;
using Local_Canteen_Optimizer.Model;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View.Order
{
    public sealed partial class Order : UserControl
    {
        private ListOrders listOrderControl;
        private ViewOrder viewOrderControl;
        public Order()
        {
            this.InitializeComponent();

            // Khởi tạo danh sách sản phẩm
            listOrderControl = new ListOrders();
            listOrderControl.ViewOrderDetailRequested += OnViewDetailRequested;

            viewOrderControl = new ViewOrder();
            viewOrderControl.CancelRequested += OnCancelRequested;

            // Hiển thị danh sách sản phẩm ban đầu
            OrdersContent.Content = listOrderControl;
        }

        private void OnCancelRequested(object sender, EventArgs e)
        {
            // Khi hủy, quay lại danh sách sản phẩm
            OrdersContent.Content = listOrderControl;
        }

        private async void OnViewDetailRequested(object sender, OrderModel order)
        {
            await viewOrderControl.SetOrder(listOrderControl.orderViewModel,order);
            OrdersContent.Content = viewOrderControl;
        }
    }
}
