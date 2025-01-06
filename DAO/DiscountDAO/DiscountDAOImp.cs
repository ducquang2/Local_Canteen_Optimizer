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
using static Local_Canteen_Optimizer.DAO.OrderDAO.OrderDAOImp;

namespace Local_Canteen_Optimizer.DAO.DiscountDAO
{
    /// <summary>
    /// Implementation of the IDiscountDAO interface.
    /// </summary>
    public class DiscountDAOImp : IDiscountDAO
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscountDAOImp"/> class.
        /// </summary>
        public DiscountDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        /// <summary>
        /// Gets the eligible discounts based on the total price.
        /// </summary>
        /// <param name="totalPrice">The total price.</param>
        /// <returns>A list of eligible discounts.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<List<DiscountModel>> GetEligibleDiscount(double totalPrice)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GetEligibleDiscountResponse>($"api/v1/discount/eligible?totalPrice={totalPrice}");
                var discounts = response.discounts.Select(item => new DiscountModel
                {
                    DiscountID = item.promotion_id.ToString(),
                    DiscountName = item.promotion_name.ToString(),
                    DiscountDescription = item.description.ToString(),
                    DiscountType = item.discount_type,
                    DiscountValue = Double.Parse(item.discount_value),
                    DiscountMinOrderValue = item.min_order_value,
                    DiscountMaxValue = item.max_discount_amount,
                    DiscountStartDate = item.start_date,
                    DiscountEndDate = item.end_date,
                    DiscountAmount = Double.Parse(item.discount_amount)
                }).ToList();

                return discounts;
            }
            catch
            {
                // Handle errors if any
                return null;
            }
        }

        /// <summary>
        /// Gets the discounts asynchronously.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="rowsPerPage">The number of rows per page.</param>
        /// <param name="keyword">The search keyword.</param>
        /// <param name="startDateAscending">if set to <c>true</c> [start date ascending].</param>
        /// <returns>A tuple containing the total items and a list of discounts.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<Tuple<int, List<DiscountModel>>> GetDiscountsAsync(int? page, int? rowsPerPage, string keyword, bool startDateAscending)
        {
            try
            {
                var sortOrder = startDateAscending ? "asc" : "desc";
                var url = $"api/v1/discounts?page={page}&pageSize={rowsPerPage}&search={keyword}&sort={sortOrder}";
                var products = await _httpClient.GetFromJsonAsync<GetApiResponse>(url);
                var discounts = products.Results.Select(ConvertToDiscountModel).ToList();
                return new Tuple<int, List<DiscountModel>>(products.TotalItems, discounts);
            }
            catch
            {
                // Handle errors if any
                return null;
            }
        }

        /// <summary>
        /// Adds the discount asynchronously.
        /// </summary>
        /// <param name="newDiscount">The new discount.</param>
        /// <returns>The added discount model.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<DiscountModel> AddDiscountAsync(DiscountModel newDiscount)
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    var createDiscountRequest = new
                    {
                        promotion_name = newDiscount.DiscountName,
                        description = newDiscount.DiscountDescription,
                        discount_type = newDiscount.DiscountType,
                        discount_value = newDiscount.DiscountValue,
                        min_order_value = newDiscount.DiscountMinOrderValue,
                        max_discount_amount = newDiscount.DiscountMaxValue,
                        start_date = newDiscount.DiscountStartDate,
                        end_date = newDiscount.DiscountEndDate,
                    };

                    var response = await _httpClient.PostAsJsonAsync("api/v1/discount", createDiscountRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        var addedProduct = await response.Content.ReadFromJsonAsync<AddApiResponse>();
                        return ConvertToDiscountModel(addedProduct.discount);
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
        /// Updates the discount asynchronously.
        /// </summary>
        /// <param name="newDiscount">The new discount.</param>
        /// <returns>The updated discount model.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<DiscountModel> UpdateDiscountAsync(DiscountModel newDiscount)
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    var editDiscountRequest = new
                    {
                        promotion_name = newDiscount.DiscountName,
                        description = newDiscount.DiscountDescription,
                        discount_type = newDiscount.DiscountType,
                        discount_value = newDiscount.DiscountValue,
                        min_order_value = newDiscount.DiscountMinOrderValue,
                        max_discount_amount = newDiscount.DiscountMaxValue,
                        start_date = newDiscount.DiscountStartDate,
                        end_date = newDiscount.DiscountEndDate,
                    };

                    var response = await _httpClient.PutAsJsonAsync($"api/v1/discount/{newDiscount.DiscountID}", editDiscountRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        var editedDiscount = await response.Content.ReadFromJsonAsync<AddApiResponse>(); // add api response same structure with update api response
                        return ConvertToDiscountModel(editedDiscount.discount);
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
        /// Removes the discount asynchronously.
        /// </summary>
        /// <param name="discountId">The discount identifier.</param>
        /// <returns><c>true</c> if the discount was removed; otherwise, <c>false</c>.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<bool> RemoveDiscountAsync(int discountId)
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    var response = await _httpClient.DeleteAsync($"api/v1/discount/{discountId}");

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
        /// Converts the API discount to a discount model.
        /// </summary>
        /// <param name="apiDiscount">The API discount.</param>
        /// <returns>The discount model.</returns>
        [ArmDot.Client.VirtualizeCode]
        private DiscountModel ConvertToDiscountModel(ApiDiscount apiDiscount)
        {
            return new DiscountModel
            {
                DiscountID = apiDiscount.promotion_id.ToString(),
                DiscountName = apiDiscount.promotion_name,
                DiscountDescription = apiDiscount.description,
                DiscountType = apiDiscount.discount_type,
                DiscountValue = Double.Parse(apiDiscount.discount_value),
                DiscountMinOrderValue = apiDiscount.min_order_value,
                DiscountMaxValue = apiDiscount.max_discount_amount,
                DiscountStartDate = apiDiscount.start_date,
                DiscountEndDate = apiDiscount.end_date,
            };
        }

        /// <summary>
        /// Response class for getting eligible discounts.
        /// </summary>
        public class GetEligibleDiscountResponse
        {
            public List<ApiDiscount> discounts { get; set; }
        }

        /// <summary>
        /// Response class for getting discounts.
        /// </summary>
        public class GetApiResponse
        {
            [JsonPropertyName("totalItems")]
            public int TotalItems { get; set; }

            [JsonPropertyName("results")]
            public List<ApiDiscount> Results { get; set; }
        }

        /// <summary>
        /// Response class for adding a discount.
        /// </summary>
        public class AddApiResponse
        {
            public ApiDiscount discount { get; set; }
        }
    }
}
