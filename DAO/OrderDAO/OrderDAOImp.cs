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
using Windows.Media.Playback;
using static Local_Canteen_Optimizer.DAO.OrderDAO.OrderDAOImp;
using static Local_Canteen_Optimizer.DAO.ProductDAO.ProductDAOImp;
using static Local_Canteen_Optimizer.DAO.SeatDAO.SeatDAOImp;

namespace Local_Canteen_Optimizer.DAO.OrderDAO
{
    /// <summary>
    /// Implementation of the IOrderDAO interface.
    /// </summary>
    public class OrderDAOImp : IOrderDAO
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderDAOImp"/> class.
        /// </summary>
        public OrderDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        /// <summary>
        /// Adds a new order asynchronously.
        /// </summary>
        /// <param name="orderModel">The order model.</param>
        /// <returns>The added order model.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<OrderModel> AddOrderAsync(OrderModel orderModel)
        {
            try
            {
                ApiOrder apiOrder = new ApiOrder
                {
                    order_status = "pending",
                    note = orderModel.Note,
                    //total_price = orderModel.Total,
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
                            product_id = int.Parse(orderModel.OrderDetails[i].ProductID),
                            quantity = orderModel.OrderDetails[i].QuantityBuy,
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

        /// <summary>
        /// Checks out an order asynchronously.
        /// </summary>
        /// <param name="tableId">The table ID.</param>
        /// <param name="orderId">The order ID.</param>
        /// <returns>True if checkout is successful, otherwise false.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<bool> CheckOut(int tableId, int orderId)
        {
            try
            {
                var checkOutRequest = new
                {
                    table_id = tableId,
                    order_id = orderId,
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

        /// <summary>
        /// Gets all order items asynchronously.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <returns>A list of food models.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<List<FoodModel>> GetAllOrderItems(int orderId)
        {
            try
            {
               
                var response = await _httpClient.GetFromJsonAsync<GetApiOrderDetailsResponse>($"api/v1/orders/{orderId}/items");
                var orderDetails = response.items.Select(item => new FoodModel
                {
                    ProductID = item.product_id.ToString(),
                    ImageSource = item.image_url,
                    Name = item.product_name.ToString(),
                    Price = item.price,
                    Quantity = item.quantity
                }).ToList();
                return orderDetails;
                
            }
            catch
            {
                // Xử lý lỗi nếu có
                return null;
            }
        }

        /// <summary>
        /// Gets all orders asynchronously.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="rowsPerPage">The number of rows per page.</param>
        /// <param name="dateAscending">Sort order by date.</param>
        /// <returns>A tuple containing total items and a list of order models.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<Tuple<int, List<OrderModel>>> GetAllOrders(int? page, int? rowsPerPage, bool dateAscending)
        {
            try
            {
                var sortOrder = dateAscending ? "asc" : "desc";
                var url = $"api/v1/orders?page={page}&pageSize={rowsPerPage}&sort={sortOrder}";
                var response = await _httpClient.GetFromJsonAsync<GetListOrderResponse>(url);
                var orders = response.order.Select(ConvertToOrderModel).ToList();
                return new Tuple<int, List<OrderModel>>(response.TotalItems, orders);
            }
            catch
            {
                // Xử lý lỗi nếu có
                return null;
            }
        }

        /// <summary>
        /// Gets the order model from a table asynchronously.
        /// </summary>
        /// <param name="tableId">The table ID.</param>
        /// <returns>The order model.</returns>
        [ArmDot.Client.VirtualizeCode]
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

        /// <summary>
        /// Updates order items asynchronously.
        /// </summary>
        /// <param name="orderModel">The order model.</param>
        /// <returns>True if update is successful, otherwise false.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<bool> UpdateOrderItems(OrderModel orderModel)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/v1/order-items/{orderModel.OrderId}");
                if (response.IsSuccessStatusCode) { 

                    for (int i = 0; i < orderModel.OrderDetails.Count; i++)
                    {
                        ApiOrderItem apiOrderItem = new ApiOrderItem
                        {
                            product_id = int.Parse(orderModel.OrderDetails[i].ProductID),
                            quantity = orderModel.OrderDetails[i].QuantityBuy,
                            price = orderModel.OrderDetails[i].Price
                        };
                        var response2 = await _httpClient.PostAsJsonAsync($"api/v1/orders/{orderModel.OrderId}/items", apiOrderItem);
                        if (!response2.IsSuccessStatusCode)
                        {
                            // Xử lý lỗi từ server
                            var errorContent = await response2.Content.ReadAsStringAsync();
                            Console.WriteLine($"Error: {errorContent}");
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch
            {
                // Xử lý lỗi nếu có
                return false;
            }
        }

        /// <summary>
        /// Updates order asynchronously.
        /// </summary>
        /// <param name="orderModel">The order model.</param>
        /// <returns>True if update is successful, otherwise false.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<bool> UpdateOrder(OrderModel orderModel)
        {
            ApiOrder apiOrder = new ApiOrder
            {
                note = orderModel.Note,
            };
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/v1/orders/{orderModel.OrderId}", apiOrder);
                if (response.IsSuccessStatusCode) {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                // Xử lý lỗi nếu có
                return false;
            }
        }

        /// <summary>
        /// Updates the table after an order asynchronously.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <param name="tableId">The table ID.</param>
        /// <returns>True if update is successful, otherwise false.</returns>
        [ArmDot.Client.VirtualizeCode]
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

        /// <summary>
        /// Adds reward points to a customer asynchronously.
        /// </summary>
        /// <param name="totalPrice">The total price.</param>
        /// <param name="customerId">The customer ID.</param>
        /// <returns>True if reward points are added successfully, otherwise false.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<bool> AddRewardPoints(double totalPrice, int customerId)
        {
            try
            {
                var addRewardPointsRequest = new
                {
                    total_price = totalPrice,
                };
                var response = await _httpClient.PostAsJsonAsync($"api/v1/earn-points/{customerId}", addRewardPointsRequest);

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

        /// <summary>
        /// Converts an API order to an order model.
        /// </summary>
        /// <param name="apiOrder">The API order.</param>
        /// <returns>The order model.</returns>
        [ArmDot.Client.VirtualizeCode]
        private OrderModel ConvertToOrderModel(ApiOrder apiOrder)
        {
            return new OrderModel
            {
                OrderId = apiOrder.order_id,
                OrderTime = apiOrder.created_at,
                Total = apiOrder.total_price,
                OrderStatus = apiOrder.order_status.ToString(),
                Note = apiOrder.note,
                DiscountPrice = apiOrder.discount_price,
                RewardPoints = apiOrder.reward_value_used,
                FinalPrice = apiOrder.final_price
            };
        }

        /// <summary>
        /// Converts an API order details response to an order model with items.
        /// </summary>
        /// <param name="response">The API order details response.</param>
        /// <returns>The order model with items.</returns>
        [ArmDot.Client.VirtualizeCode]
        private OrderModel ConvertToOrderModelWithItems(GetApiOrderDetailsResponse response)
        {
            return new OrderModel
            {
                OrderId = response.order.order_id,
                OrderTime = response.order.created_at,
                Total = response.order.total_price,
                OrderStatus = response.order.order_status.ToString(),
                Note = response.order.note,
                OrderDetails = response.items.Select(item => new FoodModel
                {
                    ProductID = item.product_id.ToString(),
                    ImageSource = item.image_url,
                    Name = item.product_name.ToString(),
                    Price = item.price,
                    Quantity = item.quantity,
                    QuantityBuy = item.quantity
                }).ToList()
            };
        }

        /// <summary>
        /// Applies a discount to an order asynchronously.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <param name="promotionId">The promotion ID.</param>
        /// <returns>The discount amount if successful, otherwise null.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<double?> ApplyDiscount(int orderId, int promotionId)
        {
            try
            {
                var applyDiscountRequest = new
                {           
                    orderId = orderId,
                    promotionId = promotionId
                };
                var response = await _httpClient.PostAsJsonAsync($"api/v1/discount/apply-discount", applyDiscountRequest);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadFromJsonAsync<GetDiscountAmount>();
                    return responseContent.discountAmount;
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

        /// <summary>
        /// Applies reward points to an order asynchronously.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <param name="totalPrice">The total price.</param>
        /// <param name="phoneNumber">The customer's phone number.</param>
        /// <param name="rewardPoints">The reward points to apply.</param>
        /// <returns>True if reward points are applied successfully, otherwise false.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<bool> ApplyRewardPoint(int orderId, double totalPrice, string phoneNumber, int rewardPoints)
        {
            try
            {
                var applyRewardPointRequest = new
                {
                    order_id = orderId,
                    final_price = totalPrice,
                    customer_phone_number = phoneNumber,
                    points = rewardPoints,
                };
                var response = await _httpClient.PostAsJsonAsync($"api/v1/orders/apply-reward-points", applyRewardPointRequest);

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

        /// <summary>
        /// Represents the API response for an order.
        /// </summary>
        public class GetApiResponse
        {
            public ApiOrder order { get; set; }
        }

        /// <summary>
        /// Represents the discount amount.
        /// </summary>
        public class GetDiscountAmount
        {
            [JsonPropertyName("discount")]
            public double discountAmount { get; set; }
        }

        /// <summary>
        /// Represents the list order response.
        /// </summary>
        public class GetListOrderResponse
        {
            [JsonPropertyName("totalItems")]
            public int TotalItems { get; set; }
            [JsonPropertyName("results")]
            public List<ApiOrder> order { get; set; }
        }

        /// <summary>
        /// Represents the API order details response.
        /// </summary>
        public class GetApiOrderDetailsResponse
        {
            [JsonPropertyName("order")]
            public ApiOrder order { get; set; }

            [JsonPropertyName("items")]
            public List<ApiOrderItem> items { get; set; }

        }
    }
}
