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
    /// A UserControl for editing customer details.
    /// </summary>
    public sealed partial class EditCustomer : UserControl
    {
        /// <summary>
        /// Event triggered when the save button is clicked.
        /// </summary>
        public event EventHandler<CustomerModel> SaveRequested;

        /// <summary>
        /// Event triggered when the cancel button is clicked.
        /// </summary>
        public event EventHandler CancelRequested;

        private CustomerModel currentCustomer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditCustomer"/> class.
        /// </summary>
        public EditCustomer()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Sets the customer details to be edited.
        /// </summary>
        /// <param name="customer">The customer model.</param>
        public void SetCustomer(CustomerModel customer)
        {
            currentCustomer = customer;
            NameTextBox.Text = customer.FullName;
            EmailTextBox.Text = customer.Email;
            PhoneTextBox.Text = customer.PhoneNumber;
            AddressTextBox.Text = customer.Address;
            RewardTextBox.Text = customer.RewardPoints.ToString();
        }

        /// <summary>
        /// Handles the save button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;

            // Reset error messages
            NameErrorText.Visibility = Visibility.Collapsed;
            EmailErrorText.Visibility = Visibility.Collapsed;
            PhoneErrorText.Visibility = Visibility.Collapsed;
            AddressErrorText.Visibility = Visibility.Collapsed;
            RewardErrorText.Visibility = Visibility.Collapsed;

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

            // Validate Reward Points
            if (!int.TryParse(RewardTextBox.Text, out var reward) || reward < 0)
            {
                RewardErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // If there are errors, stop here
            if (hasError) return;

            currentCustomer.FullName = NameTextBox.Text;
            currentCustomer.Email = EmailTextBox.Text;
            currentCustomer.PhoneNumber = PhoneTextBox.Text;
            currentCustomer.Address = AddressTextBox.Text;
            currentCustomer.RewardPoints = reward;

            SaveRequested?.Invoke(this, currentCustomer);
        }

        /// <summary>
        /// Handles the cancel button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
