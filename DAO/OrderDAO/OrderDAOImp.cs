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
using static Local_Canteen_Optimizer.DAO.ProductDAO.ProductDAOImp;

namespace Local_Canteen_Optimizer.DAO.OrderDAO
{
    public class OrderDAOImp : IOrderDAO
    {
        private readonly HttpClient _httpClient;

        public OrderDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }
        public async Task<bool> AddOrderAsync(OrderModel orderModel)
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    //localSettings.Values.Remove("userToken");
                    string userToken = localSettings.Values["userToken"] as string;
                    //string userToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImFkbWluMDEiLCJyb2xlIjoiYWRtaW4iLCJpYXQiOjE3MzA2MTE2NTMsImV4cCI6MTczMDYyMjQ1M30.0Cm-GNsXjkXFDbCEgDyCod725zC0Q7GP5YoM2mIIl2k";
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    ApiOrder apiOrder = new ApiOrder
                    {
                        order_status = "pending",
                        total_price = orderModel.Total,
                    };

                    var response = await _httpClient.PostAsJsonAsync("api/v1/orders/add", apiOrder);

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadFromJsonAsync<GetApiResponse>();
                        var order_id = apiResponse.order.order_id;
                        for (int i = 0; i < orderModel.OrderDetails.Count; i++)
                        {
                            ApiOrderItem apiOrderItem = new ApiOrderItem
                            {
                                product_id = int.Parse(orderModel.OrderDetails[i].Id),
                                quantity = 1,
                                price = orderModel.OrderDetails[i].Price
                            };
                            var response2 = await _httpClient.PostAsJsonAsync($"api/v1/orders-item/add/{order_id}" , apiOrderItem);
                            if (!response2.IsSuccessStatusCode)
                            {
                                // Xử lý lỗi từ server
                                var errorContent = await response2.Content.ReadAsStringAsync();
                                Console.WriteLine($"Error: {errorContent}");
                                return false;
                            }
                        }
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

        public class GetApiResponse
        {
            public ApiOrder order { get; set; }
        }
    }
}
