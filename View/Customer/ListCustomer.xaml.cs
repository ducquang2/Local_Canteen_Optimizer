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
    public sealed partial class ListCustomer : UserControl
    {
        CustomerViewModel customerViewModel;
        public event EventHandler AddCustomerRequested;
        public event EventHandler<CustomerModel> EditCustomerRequested;
        public ListCustomer()
        {
            this.InitializeComponent();
            InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            customerViewModel = new CustomerViewModel();
            await customerViewModel.Init();
            UpdatePagingInfo_bootstrap();
        }

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

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            AddCustomerRequested?.Invoke(this, EventArgs.Empty);
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy sản phẩm từ nút Edit
            var customer = (sender as Button).Tag as CustomerModel;
            EditCustomerRequested?.Invoke(this, customer);
        }
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
        public async void UpdateCustomer(CustomerModel customer)
        {
            await customerViewModel.UpdateCustomer(customer);
        }

        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is CustomerModel customer)
            {
                await customerViewModel.DeleteCustomer(customer);
            }
        }

        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic item = pagesComboBox.SelectedItem;

            if (item != null)
            {
                customerViewModel.Load(item.Page);
            }
        }

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


        private void keywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            await handleSearchButtonClick();
        }

        public async Task handleSearchButtonClick()
        {
            await customerViewModel.Load(1);
            UpdatePagingInfo_bootstrap();
        }
    }
}
