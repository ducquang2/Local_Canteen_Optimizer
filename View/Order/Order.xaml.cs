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
    /// <summary>
    /// Represents the Order view control.
    /// </summary>
    public sealed partial class Order : UserControl
    {
        private ListOrders listOrderControl;
        private ViewOrder viewOrderControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        public Order()
        {
            this.InitializeComponent();

            // Initialize the list of products
            listOrderControl = new ListOrders();
            listOrderControl.ViewOrderDetailRequested += OnViewDetailRequested;

            viewOrderControl = new ViewOrder();
            viewOrderControl.CancelRequested += OnCancelRequested;

            // Display the initial list of products
            OrdersContent.Content = listOrderControl;
        }

        /// <summary>
        /// Handles the CancelRequested event to return to the list of products.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCancelRequested(object sender, EventArgs e)
        {
            // When canceled, return to the list of products
            OrdersContent.Content = listOrderControl;
        }

        /// <summary>
        /// Handles the ViewOrderDetailRequested event to display the order details.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="order">The order model.</param>
        private async void OnViewDetailRequested(object sender, OrderModel order)
        {
            await viewOrderControl.SetOrder(listOrderControl.orderViewModel, order);
            OrdersContent.Content = viewOrderControl;
        }
    }
}
