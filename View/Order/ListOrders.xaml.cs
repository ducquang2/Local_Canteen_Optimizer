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

namespace Local_Canteen_Optimizer.View.Order
{
    public sealed partial class ListOrders : UserControl
    {
        OrderViewModel orderViewModel;
        public event EventHandler<OrderModel> ViewOrderDetailRequested;
        public ListOrders()
        {
            this.InitializeComponent();
            orderViewModel = new OrderViewModel();
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            // lấy sản phẩm từ nút View
            var order = (sender as Button).Tag as OrderModel;
            ViewOrderDetailRequested?.Invoke(this, order);
        }
    }
}
