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
    public class CustomerDAOImp : ICustomerDAO
    {
        private readonly HttpClient _httpClient;
        public CustomerDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        public async Task<CustomerModel> GetCustomerByPhoneNumber(String phoneNumber)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GetCustomerResponse>($"api/v1/customer/{phoneNumber}");
                return ConvertToCustomerModel(response.customer);
            }
            catch
            {
                // Xử lý lỗi nếu có
                return null;
            }
        }

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
                        // Xử lý lỗi từ server
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
                // Xử lý lỗi nếu có
                return null;
            }

        }

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
                        // Xử lý lỗi từ server
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
                // Xử lý lỗi nếu có
                return false;
            }
        }

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
                        // Xử lý lỗi từ server
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
                // Xử lý lỗi nếu có
                return null;
            }
        }

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

        public class GetApiResponse
        {
            [JsonPropertyName("totalItems")]
            public int TotalItems { get; set; }

            [JsonPropertyName("results")]
            public List<ApiCustomer> Results { get; set; }
        }

        public class AddApiResponse
        {
            [JsonPropertyName("customer")]
            public ApiCustomer customer { get; set; }
        }

        public class GetCustomerResponse
        {
            public ApiCustomer customer { get; set; }
        }
    }
}
