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
    /// <summary>
    /// A UserControl that manages customer-related operations.
    /// </summary>
    public sealed partial class Customers : UserControl
    {
        private ListCustomer customerListControl;
        private AddCustomer addCustomerControl;
        private EditCustomer editCustomerControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="Customers"/> class.
        /// </summary>
        public Customers()
        {
            this.InitializeComponent();

            // Initialize customer list control
            customerListControl = new ListCustomer();
            customerListControl.AddCustomerRequested += OnAddCustomerRequested;
            customerListControl.EditCustomerRequested += OnEditCustomerRequested;

            // Initialize add customer control
            addCustomerControl = new AddCustomer();
            addCustomerControl.SaveRequested += OnAddSaveRequested;
            addCustomerControl.CancelRequested += OnCancelRequested;

            // Initialize edit customer control
            editCustomerControl = new EditCustomer();
            editCustomerControl.SaveRequested += OnEditSaveRequested;
            editCustomerControl.CancelRequested += OnCancelRequested;

            // Display initial customer list
            CustomersContent.Content = customerListControl;
        }

        /// <summary>
        /// Handles the AddCustomerRequested event.
        /// </summary>
        private void OnAddCustomerRequested(object sender, EventArgs e)
        {
            // Switch to add customer form
            CustomersContent.Content = addCustomerControl;
        }

        /// <summary>
        /// Handles the EditCustomerRequested event.
        /// </summary>
        private void OnEditCustomerRequested(object sender, CustomerModel customer)
        {
            editCustomerControl.SetCustomer(customer);
            CustomersContent.Content = editCustomerControl;
        }

        /// <summary>
        /// Handles the SaveRequested event from the add customer control.
        /// </summary>
        private void OnAddSaveRequested(object sender, CustomerModel customer)
        {
            // Add customer to list and switch back to customer list
            customerListControl.AddCustomer(customer);
            CustomersContent.Content = customerListControl;
        }

        /// <summary>
        /// Handles the SaveRequested event from the edit customer control.
        /// </summary>
        private void OnEditSaveRequested(object sender, CustomerModel customer)
        {
            customerListControl.UpdateCustomer(customer);
            CustomersContent.Content = customerListControl; // Switch back to customer list
        }

        /// <summary>
        /// Handles the CancelRequested event.
        /// </summary>
        private void OnCancelRequested(object sender, EventArgs e)
        {
            // Switch back to customer list
            CustomersContent.Content = customerListControl;
        }
    }
}
