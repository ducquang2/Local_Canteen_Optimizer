using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.CustomerDAO
{
    /// <summary>
    /// Implementation of the Customer Data Access Object (DAO) interface.
    /// </summary>
    public class CustomerDAOImp : ICustomerDAO
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerDAOImp"/> class.
        /// </summary>
        public CustomerDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        /// <summary>
        /// Gets a customer by phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number of the customer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the customer model.</returns>
        public async Task<CustomerModel> GetCustomerByPhoneNumber(String phoneNumber)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GetCustomerResponse>($"api/v1/customer/{phoneNumber}");
                return ConvertToCustomerModel(response.customer);
            }
            catch
            {
                // Handle errors if any
                return null;
            }
        }

        /// <summary>
        /// Adds a new customer asynchronously.
        /// </summary>
        /// <param name="newCustomer">The new customer model.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added customer model.</returns>
        public async Task<CustomerModel> AddCustomerAsync(CustomerModel newCustomer)
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    ApiCustomer apiCustomer = new ApiCustomer
                    {
                        full_name = newCustomer.FullName,
                        email = newCustomer.Email,
                        phone_number = newCustomer.PhoneNumber,
                        address = newCustomer.Address
                    };

                    var response = await _httpClient.PostAsJsonAsync("api/v1/customer", apiCustomer);

                    if (response.IsSuccessStatusCode)
                    {
                        var addedCustomer = await response.Content.ReadFromJsonAsync<AddApiResponse>();
                        return ConvertToCustomerModel(addedCustomer.customer);
                    }
                    else
                    {
                        // Handle server errors
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {errorContent}");
                        return null;
                    }
                }
                else
                {
                    // Handle the case where the token is not found
                    Console.WriteLine("User token not found.");
                    return null;
                }
            }
            catch
            {
                // Handle errors if any
                return null;
            }
        }

        /// <summary>
        /// Gets a list of customers asynchronously.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="rowsPerPage">The number of rows per page.</param>
        /// <param name="keyword">The search keyword.</param>
        /// <param name="nameAscending">if set to <c>true</c> [name ascending].</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with the total items and a list of customer models.</returns>
        public async Task<Tuple<int, List<CustomerModel>>> GetCustomersAsync(int? page, int? rowsPerPage, string keyword, bool nameAscending)
        {
            try
            {
                var sortOrder = nameAscending ? "asc" : "desc";
                var url = $"api/v1/customers?page={page}&pageSize={rowsPerPage}&search={keyword}&sort={sortOrder}";
                var customers = await _httpClient.GetFromJsonAsync<GetApiResponse>(url);
                var customerModels = customers.Results.Select(ConvertToCustomerModel).ToList();
                return new Tuple<int, List<CustomerModel>>(customers.TotalItems, customerModels);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Removes a customer asynchronously.
        /// </summary>
        /// <param name="customerID">The customer identifier.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.</returns>
        public async Task<bool> RemoveCustomerAsync(int customerID)
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    var response = await _httpClient.DeleteAsync($"api/v1/customer/{customerID}");

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        // Handle server errors
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {errorContent}");
                        return false;
                    }
                }
                else
                {
                    // Handle the case where the token is not found
                    Console.WriteLine("User token not found.");
                    return false;
                }
            }
            catch
            {
                // Handle errors if any
                return false;
            }
        }

        /// <summary>
        /// Updates a customer asynchronously.
        /// </summary>
        /// <param name="newCustomer">The new customer model.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated customer model.</returns>
        public async Task<CustomerModel> UpdateCustomerAsync(CustomerModel newCustomer)
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    ApiCustomer apiCustomer = new ApiCustomer
                    {
                        full_name = newCustomer.FullName,
                        email = newCustomer.Email,
                        phone_number = newCustomer.PhoneNumber,
                        address = newCustomer.Address,
                        reward_points = newCustomer.RewardPoints
                    };

                    var response = await _httpClient.PutAsJsonAsync($"api/v1/customer/{newCustomer.CustomerID}", apiCustomer);

                    if (response.IsSuccessStatusCode)
                    {
                        var addedCustomer = await response.Content.ReadFromJsonAsync<AddApiResponse>(); // add api response same structure with update api response
                        return ConvertToCustomerModel(addedCustomer.customer);
                    }
                    else
                    {
                        // Handle server errors
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {errorContent}");
                        return null;
                    }
                }
                else
                {
                    // Handle the case where the token is not found
                    Console.WriteLine("User token not found.");
                    return null;
                }
            }
            catch
            {
                // Handle errors if any
                return null;
            }
        }

        /// <summary>
        /// Converts the API customer model to the customer model.
        /// </summary>
        /// <param name="apiCustomer">The API customer model.</param>
        /// <returns>The customer model.</returns>
        private CustomerModel ConvertToCustomerModel(ApiCustomer apiCustomer)
        {
            return new CustomerModel
            {
                CustomerID = apiCustomer.customer_id,
                FullName = apiCustomer.full_name,
                Email = apiCustomer.email,
                PhoneNumber = apiCustomer.phone_number,
                Address = apiCustomer.address,
                RewardPoints = apiCustomer.reward_points,
                createAt = apiCustomer.created_at
            };
        }

        /// <summary>
        /// Response model for getting customers.
        /// </summary>
        public class GetApiResponse
        {
            [JsonPropertyName("totalItems")]
            public int TotalItems { get; set; }

            [JsonPropertyName("results")]
            public List<ApiCustomer> Results { get; set; }
        }

        /// <summary>
        /// Response model for adding a customer.
        /// </summary>
        public class AddApiResponse
        {
            [JsonPropertyName("customer")]
            public ApiCustomer customer { get; set; }
        }

        /// <summary>
        /// Response model for getting a customer by phone number.
        /// </summary>
        public class GetCustomerResponse
        {
            public ApiCustomer customer { get; set; }
        }
    }
}
