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
    /// <summary>
    /// Interaction logic for ViewOrder.xaml
    /// </summary>
    public sealed partial class ViewOrder : UserControl
    {
        /// <summary>
        /// Event triggered when the cancel button is clicked.
        /// </summary>
        public event EventHandler CancelRequested;

        /// <summary>
        /// The current order view model.
        /// </summary>
        private OrderViewModel currentOrderViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewOrder"/> class.
        /// </summary>
        public ViewOrder()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Sets the order view model and updates the order model.
        /// </summary>
        /// <param name="orderViewModel">The order view model.</param>
        /// <param name="order">The order model.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SetOrder(OrderViewModel orderViewModel, OrderModel order)
        {
            currentOrderViewModel = orderViewModel;
            await currentOrderViewModel.UpdateOrderModel(order);
        }

        /// <summary>
        /// Handles the click event of the cancel button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
