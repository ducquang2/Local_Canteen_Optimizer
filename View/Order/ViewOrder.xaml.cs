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
using Local_Canteen_Optimizer.ViewModel;
using DocumentFormat.OpenXml.Vml;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View.Order
{
    public sealed partial class ViewOrder : UserControl
    {
        public event EventHandler CancelRequested;
        private OrderViewModel currentOrderViewModel;
        public ViewOrder()
        {
            this.InitializeComponent();
        }
        public async Task SetOrder(OrderViewModel orderViewModel,OrderModel order)
        {
            currentOrderViewModel = orderViewModel;
            await currentOrderViewModel.UpdateOrderModel(order);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
