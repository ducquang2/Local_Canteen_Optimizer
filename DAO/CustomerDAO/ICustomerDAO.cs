using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.CustomerDAO
{
    public interface ICustomerDAO
    {
        public Task<CustomerModel> GetCustomerByPhoneNumber(String phoneNumber);
        public Task<CustomerModel> AddCustomerAsync(CustomerModel newCustomer);
        public Task<Tuple<int, List<CustomerModel>>> GetCustomersAsync(int? page, int? rowsPerPage, string keyword, bool nameAscending);
        public Task<bool> RemoveCustomerAsync(int customerID);
        public Task<CustomerModel> UpdateCustomerAsync(CustomerModel newCustomer);
    }
}
