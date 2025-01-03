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
    public sealed partial class EditCustomer : UserControl
    {
        public event EventHandler<CustomerModel> SaveRequested;
        public event EventHandler CancelRequested;
        private CustomerModel currentCustomer;
        public EditCustomer()
        {
            this.InitializeComponent();
        }
        public void SetCustomer(CustomerModel customer)
        {
            currentCustomer = customer;
            NameTextBox.Text = customer.FullName;
            EmailTextBox.Text = customer.Email;
            PhoneTextBox.Text = customer.PhoneNumber;
            AddressTextBox.Text = customer.Address;
            RewardTextBox.Text = customer.RewardPoints.ToString();
        }
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

            // Validate Image Source
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                EmailErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // Validate Price
            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                PhoneErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // Validate Quantity
            if (string.IsNullOrWhiteSpace(AddressTextBox.Text))
            {
                AddressErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // Validate Quantity
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
