using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.Service;
using Local_Canteen_Optimizer.View.Product;
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
using static Local_Canteen_Optimizer.DAO.ProductDAO.ProductDAOImp;
using static Local_Canteen_Optimizer.DAO.SeatDAO.SeatDAOImp;

namespace Local_Canteen_Optimizer.DAO.OrderDAO
{
    public class OrderDAOImp : IOrderDAO
    {
        private readonly HttpClient _httpClient;

        public OrderDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }
        public async Task<OrderModel> AddOrderAsync(OrderModel orderModel)
        {
            try
            {
                ApiOrder apiOrder = new ApiOrder
                {
                    order_status = "pending",
                    total_price = orderModel.Total,
                };

                var response = await _httpClient.PostAsJsonAsync("api/v1/orders", apiOrder);

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
                        var response2 = await _httpClient.PostAsJsonAsync($"api/v1/orders/{order_id}/items" , apiOrderItem);
                        if (!response2.IsSuccessStatusCode)
                        {
                            // Xử lý lỗi từ server
                            var errorContent = await response2.Content.ReadAsStringAsync();
                            Console.WriteLine($"Error: {errorContent}");
                            return null;
                        }
                    }
                    return ConvertToOrderModel(apiResponse.order);
                }
                else
                {
                    // Xử lý lỗi từ server
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {errorContent}");
                    return null;
                }
                
            }
            catch
            {
                // Xử lý lỗi nếu có
                return null;
            }

        }

        public async Task<bool> CheckOut(int tableId)
        {
            try
            {
                var checkOutRequest = new
                {
                    table_id = tableId,
                };
                var response = await _httpClient.PostAsJsonAsync($"api/v1/orders/checkout", checkOutRequest);
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
            catch
            {
                // Xử lý lỗi nếu có
                return false;
            }
        }

        public async Task<List<CartItemModel>> GetAllOrderItems(int orderId)
        {
            try
            {
               
                var response = await _httpClient.GetFromJsonAsync<GetApiOrderDetailsResponse>($"api/v1/orders/{orderId}/items");
                var orderDetails = response.items.Select(item => new CartItemModel
                {
                    Id = item.product_id.ToString(),
                    Name = item.product_name.ToString(),
                    Price = item.price
                }).ToList();
                return orderDetails;
                
            }
            catch
            {
                // Xử lý lỗi nếu có
                return null;
            }
        }

        public async Task<List<OrderModel>> GetAllOrders()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GetListOrderResponse>($"api/v1/orders");
                var foodModels = response.order.Select(ConvertToOrderModel).ToList();
                return new List<OrderModel>(foodModels);
            }
            catch
            {
                // Xử lý lỗi nếu có
                return null;
            }
        }

        public async Task<OrderModel> GetOrderModelFromTable(int tableId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GetApiOrderDetailsResponse>($"api/v1/orders/{tableId}");
                var orderModel = ConvertToOrderModelWithItems(response);
                return orderModel;
                
            }
            catch
            {
                // Xử lý lỗi nếu có 
                return null;
            }
            
        }

        public async Task<bool> UpdateTableAfterOrder(int orderId, int tableId)
        {
            try
            {        
                var startOrderRequest = new
                {
                    table_id = tableId,
                    order_id = orderId
                };
                var response = await _httpClient.PostAsJsonAsync($"api/v1/seat/start-order", startOrderRequest);

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
            catch
            {
                // Xử lý lỗi nếu có
                return false;
            }
        }

        private OrderModel ConvertToOrderModel(ApiOrder apiOrder)
        {
            return new OrderModel
            {
                OrderId = apiOrder.order_id,
                OrderTime = apiOrder.created_at,
                Total = apiOrder.total_price,
            };
        }

        private OrderModel ConvertToOrderModelWithItems(GetApiOrderDetailsResponse response)
        {
            return new OrderModel
            {
                OrderId = response.order.order_id,
                OrderTime = response.order.created_at,
                Total = response.order.total_price,
                OrderStatus = response.order.order_status.ToString(),
                OrderDetails = response.items.Select(item => new CartItemModel
                {
                    Id = item.product_id.ToString(),
                    Name = item.product_name.ToString(),
                    Price = item.price
                }).ToList()
            };
        }

        public class GetApiResponse
        {
            public ApiOrder order { get; set; }
        }

        public class GetListOrderResponse
        {
            [JsonPropertyName("results")]
            public List<ApiOrder> order { get; set; }
        }

        public class GetApiOrderDetailsResponse
        {
            [JsonPropertyName("order")]
            public ApiOrder order { get; set; }

            [JsonPropertyName("items")]
            public List<ApiOrderItem> items { get; set; }

        }
    }
}
