using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.CustomerDAO
{
    /// <summary>
    /// Interface for Customer Data Access Object.
    /// </summary>
    public interface ICustomerDAO
    {
        /// <summary>
        /// Gets a customer by phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number of the customer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the customer model.</returns>
        public Task<CustomerModel> GetCustomerByPhoneNumber(String phoneNumber);

        /// <summary>
        /// Adds a new customer asynchronously.
        /// </summary>
        /// <param name="newCustomer">The new customer model.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added customer model.</returns>
        public Task<CustomerModel> AddCustomerAsync(CustomerModel newCustomer);

        /// <summary>
        /// Gets a list of customers asynchronously with pagination and search keyword.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="rowsPerPage">The number of rows per page.</param>
        /// <param name="keyword">The search keyword.</param>
        /// <param name="nameAscending">If set to true, sorts by name in ascending order.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with the total count and the list of customer models.</returns>
        public Task<Tuple<int, List<CustomerModel>>> GetCustomersAsync(int? page, int? rowsPerPage, string keyword, bool nameAscending);

        /// <summary>
        /// Removes a customer asynchronously.
        /// </summary>
        /// <param name="customerID">The ID of the customer to remove.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.</returns>
        public Task<bool> RemoveCustomerAsync(int customerID);

        /// <summary>
        /// Updates a customer asynchronously.
        /// </summary>
        /// <param name="newCustomer">The updated customer model.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated customer model.</returns>
        public Task<CustomerModel> UpdateCustomerAsync(CustomerModel newCustomer);
    }
}
