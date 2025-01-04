using Local_Canteen_Optimizer.Helper;
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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View.Customer
{
    /// <summary>
    /// UserControl for listing customers.
    /// </summary>
    public sealed partial class ListCustomer : UserControl
    {
        /// <summary>
        /// ViewModel for managing customer data.
        /// </summary>
        CustomerViewModel customerViewModel;

        /// <summary>
        /// Event triggered when a request to add a customer is made.
        /// </summary>
        public event EventHandler AddCustomerRequested;

        /// <summary>
        /// Event triggered when a request to edit a customer is made.
        /// </summary>
        public event EventHandler<CustomerModel> EditCustomerRequested;

        /// <summary>
        /// Initializes a new instance of the ListCustomer class.
        /// </summary>
        public ListCustomer()
        {
            this.InitializeComponent();
            InitializeAsync();
        }

        /// <summary>
        /// Asynchronously initializes the ViewModel and updates paging information.
        /// </summary>
        public async Task InitializeAsync()
        {
            customerViewModel = new CustomerViewModel();
            await customerViewModel.Init();
            UpdatePagingInfo_bootstrap();
        }

        /// <summary>
        /// Updates the paging information for the customer list.
        /// </summary>
        void UpdatePagingInfo_bootstrap()
        {
            var infoList = new List<object>();
            for (int i = 1; i <= customerViewModel.TotalPages; i++)
            {
                infoList.Add(new
                {
                    Page = i,
                    Total = customerViewModel.TotalPages
                });
            };

            pagesComboBox.ItemsSource = infoList;
            pagesComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the Add Customer button click event.
        /// </summary>
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            AddCustomerRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the Edit button click event.
        /// </summary>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var customer = (sender as Button).Tag as CustomerModel;
            EditCustomerRequested?.Invoke(this, customer);
        }

        /// <summary>
        /// Adds a new customer.
        /// </summary>
        public async void AddCustomer(CustomerModel customer)
        {
            try
            {
                await customerViewModel.AddCustomer(customer);
                await MessageHelper.ShowSuccessMessage("Add new customer successful", App.m_window.Content.XamlRoot);
            }
            catch (Exception e)
            {
                await MessageHelper.ShowErrorMessage("Fail to add new customer", App.m_window.Content.XamlRoot);
            }
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        public async void UpdateCustomer(CustomerModel customer)
        {
            await customerViewModel.UpdateCustomer(customer);
        }

        /// <summary>
        /// Handles the Remove button click event.
        /// </summary>
        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is CustomerModel customer)
            {
                await customerViewModel.DeleteCustomer(customer);
            }
        }

        /// <summary>
        /// Handles the page selection change event.
        /// </summary>
        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic item = pagesComboBox.SelectedItem;

            if (item != null)
            {
                customerViewModel.Load(item.Page);
            }
        }

        /// <summary>
        /// Handles the sort order selection change event.
        /// </summary>
        public void SortOrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var selectedItem = comboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    bool isAscending = bool.Parse(selectedItem.Tag.ToString());
                    customerViewModel.LoadCustomerSort(isAscending);
                }
            }
        }

        /// <summary>
        /// Handles the keyword text box text change event.
        /// </summary>
        private void keywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Handles the search button click event.
        /// </summary>
        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            await handleSearchButtonClick();
        }

        /// <summary>
        /// Handles the search button click event asynchronously.
        /// </summary>
        public async Task handleSearchButtonClick()
        {
            await customerViewModel.Load(1);
            UpdatePagingInfo_bootstrap();
        }
    }
}
