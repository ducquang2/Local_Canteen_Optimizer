using DocumentFormat.OpenXml.Spreadsheet;
using Local_Canteen_Optimizer.Commands;
using Local_Canteen_Optimizer.DAO.CustomerDAO;
using Local_Canteen_Optimizer.DAO.DiscountDAO;
using Local_Canteen_Optimizer.DAO.ProductDAO;
using Local_Canteen_Optimizer.Helper;
using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.View.Customer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Local_Canteen_Optimizer.ViewModel
{
    /// <summary>
    /// ViewModel class for managing customer-related operations.
    /// </summary>
    public class CustomerViewModel : BaseViewModel
    {
        private ICustomerDAO _dao = null;
        public string Keyword { get; set; } = "";
        public bool NameAcending { get; set; } = true;
        public int CurrentPage { get; set; } = 0;
        public int RowsPerPage { get; set; } = 10;
        public int TotalPages { get; set; } = 0;
        public int TotalItems { get; set; } = 0;
        public ObservableCollection<CustomerModel> customers { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }

        /// <summary>
        /// Constructor for CustomerViewModel.
        /// </summary>
        public CustomerViewModel()
        {
            _dao = new CustomerDAOImp();
        }

        /// <summary>
        /// Gets a customer by phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number of the customer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the customer model.</returns>
        public async Task<CustomerModel> getCustomerByPhone(String phoneNumber)
        {
            try
            {
                CustomerModel customer = await _dao.GetCustomerByPhoneNumber(phoneNumber);
                if (customer != null)
                {
                    return customer;
                }
                else
                {
                    await MessageHelper.ShowErrorMessage("fail to get customer", App.m_window.Content.XamlRoot);
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Initializes the CustomerViewModel.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task Init()
        {
            try
            {
                customers = new ObservableCollection<CustomerModel>();
                DeleteCustomerCommand = new RelayCommand<CustomerModel>(async (customer) => await ConfirmAndDeleteCustomer(customer));
                await LoadCustomersAsync();
            }
            catch
            {
                await MessageHelper.ShowErrorMessage("Can't get any customers", App.m_window.Content.XamlRoot);
            }
        }

        /// <summary>
        /// Loads customers for a specific page.
        /// </summary>
        /// <param name="page">The page number to load.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task Load(int page)
        {
            CurrentPage = page;
            await LoadCustomersAsync();
        }

        /// <summary>
        /// Loads customers with sorting by name.
        /// </summary>
        /// <param name="nameAcending">Boolean indicating if the sorting is ascending.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task LoadCustomerSort(bool nameAcending)
        {
            NameAcending = nameAcending;
            await LoadCustomersAsync();
        }

        /// <summary>
        /// Loads customers asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task LoadCustomersAsync()
        {
            var (totalItems, listCustomers) = await _dao.GetCustomersAsync(CurrentPage, RowsPerPage, Keyword, NameAcending);
            customers.Clear();
            foreach (var item in listCustomers)
            {
                customers.Add(item);
            }
            OnPropertyChanged(nameof(customers));

            TotalItems = totalItems;
            TotalPages = (TotalItems / RowsPerPage) + ((TotalItems % RowsPerPage == 0) ? 0 : 1);
        }

        /// <summary>
        /// Adds a new customer.
        /// </summary>
        /// <param name="customer">The customer model to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddCustomer(CustomerModel customer)
        {
            CustomerModel newCustomer = await _dao.AddCustomerAsync(customer);
            if (newCustomer != null)
            {
                customers.Add(newCustomer);
            }
            else
            {
                throw new Exception("Fail to add new customer");
            }
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="customer">The customer model to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateCustomer(CustomerModel customer)
        {
            CustomerModel updateCustomer = await _dao.UpdateCustomerAsync(customer);
            if (updateCustomer != null)
            {
                // Find and update products in the list
                var existingCustomerIndex = customers.IndexOf(customers.FirstOrDefault(p => p.CustomerID == customer.CustomerID));
                if (existingCustomerIndex >= 0)
                {
                    customers[existingCustomerIndex] = new CustomerModel
                    {
                        CustomerID = updateCustomer.CustomerID,
                        FullName = updateCustomer.FullName,
                        Email = updateCustomer.Email,
                        PhoneNumber = updateCustomer.PhoneNumber,
                        Address = updateCustomer.Address,
                        RewardPoints = updateCustomer.RewardPoints,
                        createAt = updateCustomer.createAt
                    };
                    await MessageHelper.ShowSuccessMessage("Update customer successful", App.m_window.Content.XamlRoot);
                }
                else
                {
                    await MessageHelper.ShowErrorMessage("Fail to update customer", App.m_window.Content.XamlRoot);
                }
            }
            else
            {
                await MessageHelper.ShowErrorMessage("Fail to update customer", App.m_window.Content.XamlRoot);
            }
        }

        /// <summary>
        /// Confirms and deletes a customer.
        /// </summary>
        /// <param name="customer">The customer model to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task ConfirmAndDeleteCustomer(CustomerModel customer)
        {
            if (customer == null)
            {
                return;
            }

            bool isConfirmed = await MessageHelper.ShowConfirmationDialog(
                $"Are you sure you want to delete the customer: {customer.FullName}?",
                "Confirm customer deletion",
                App.m_window.Content.XamlRoot
            );

            if (isConfirmed)
            {
                await DeleteCustomer(customer);
            }
        }

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        /// <param name="customer">The customer model to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteCustomer(CustomerModel customer)
        {
            bool isRemoved = await _dao.RemoveCustomerAsync(customer.CustomerID);
            if (isRemoved)
            {
                if (customers.Contains(customer))
                {
                    customers.Remove(customer);
                    await MessageHelper.ShowSuccessMessage("Remove customer successful", App.m_window.Content.XamlRoot);
                }
            }
            else
            {
                await MessageHelper.ShowErrorMessage("Fail to remove customer", App.m_window.Content.XamlRoot);
            }
        }
    }
}
