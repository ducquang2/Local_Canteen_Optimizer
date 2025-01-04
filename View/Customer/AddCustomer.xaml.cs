using Local_Canteen_Optimizer.Model;
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

namespace Local_Canteen_Optimizer.View.Customer
{
    /// <summary>
    /// A UserControl for adding a new customer.
    /// </summary>
    public sealed partial class AddCustomer : UserControl
    {
        /// <summary>
        /// Event triggered when the save button is clicked.
        /// </summary>
        public event EventHandler<CustomerModel> SaveRequested;

        /// <summary>
        /// Event triggered when the cancel button is clicked.
        /// </summary>
        public event EventHandler CancelRequested;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddCustomer"/> class.
        /// </summary>
        public AddCustomer()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the click event of the save button.
        /// Validates input fields and triggers the SaveRequested event if validation passes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;

            // Reset error messages
            NameErrorText.Visibility = Visibility.Collapsed;
            EmailErrorText.Visibility = Visibility.Collapsed;
            PhoneErrorText.Visibility = Visibility.Collapsed;
            AddressErrorText.Visibility = Visibility.Collapsed;

            // Validate Name
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                NameErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                EmailErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // Validate Phone
            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                PhoneErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // Validate Address
            if (string.IsNullOrWhiteSpace(AddressTextBox.Text))
            {
                AddressErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // If there are errors, stop here
            if (hasError) return;

            var product = new CustomerModel
            {
                FullName = NameTextBox.Text,
                Email = EmailTextBox.Text,
                PhoneNumber = PhoneTextBox.Text,
                Address = AddressTextBox.Text,
            };

            SaveRequested?.Invoke(this, product);
        }

        /// <summary>
        /// Handles the click event of the cancel button.
        /// Triggers the CancelRequested event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
