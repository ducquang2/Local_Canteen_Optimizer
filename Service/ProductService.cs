using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Service
{
    class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        // Phương thức GET để lấy danh sách người dùng
        public async Task<List<FoodModel>> GetProductsAsync()
        {
            try
            {
                var products = await _httpClient.GetFromJsonAsync<RootApiResponse>("products");
                return products.Results.Select(ConvertToDTO).ToList() ?? new List<FoodModel>();
            }
            catch
            {
                // Xử lý lỗi nếu có
                return new List<FoodModel>();
            }
        }

        private FoodModel ConvertToDTO(ApiProduct apiProduct)
        {
            return new FoodModel
            {
                ProductID = apiProduct.product_id.ToString(),
                Name = apiProduct.product_name,
                ImageSource = apiProduct.image_url,
                Price = apiProduct.price,
                Quantity = apiProduct.stock_quantity,
            };
            
        }

        public class RootApiResponse
        {
            [JsonPropertyName("results")]
            public List<ApiProduct> Results { get; set; }
        }
    }
}
