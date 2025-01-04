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
    /// <summary>
    /// UserControl for selling products.
    /// </summary>
    public sealed partial class SellProduct : UserControl
    {
        private Home homeControl;
        private Table tableControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SellProduct"/> class.
        /// </summary>
        public SellProduct()
        {
            this.InitializeComponent();

            // Initialize home page
            homeControl = new Home();
            homeControl.CartViewControl.AddTableRequested += OnAddTableRequested;
            homeControl.CartViewControl.HoldCartRequested += OnHoldCartRequested;
            homeControl.CartViewControl.CheckOutRequested += OnCheckOutRequested;

            // Initialize table page
            tableControl = new Table();
            tableControl.SaveTableRequested += OnSaveTableRequested;
            tableControl.CancelRequested += OnCancelRequested;

            // Display home page
            SellProductContent.Content = homeControl;
        }

        /// <summary>
        /// Handles the AddTableRequested event.
        /// </summary>
        private void OnAddTableRequested(object sender, EventArgs e)
        {
            // Switch to table selection screen when Add Table button is clicked
            SellProductContent.Content = tableControl;
        }

        /// <summary>
        /// Handles the SaveTableRequested event.
        /// </summary>
        private void OnSaveTableRequested(object sender, int tableId)
        {
            homeControl.CartViewModel.SelectedTableId = tableId;
            SellProductContent.Content = homeControl;
        }

        /// <summary>
        /// Handles the HoldCartRequested event.
        /// </summary>
        private void OnHoldCartRequested(object sender, TableModel table)
        {
            tableControl.tableViewModel.updateTable(table);
        }

        /// <summary>
        /// Handles the CheckOutRequested event.
        /// </summary>
        private void OnCheckOutRequested(object sender, TableModel table)
        {
            tableControl.tableViewModel.updateTable(table);
        }

        /// <summary>
        /// Handles the CancelRequested event.
        /// </summary>
        private void OnCancelRequested(object sender, EventArgs e)
        {
            // Return to product list when cancel is clicked
            SellProductContent.Content = homeControl;
        }
    }
}
