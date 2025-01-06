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
    /// <summary>
    /// Service class for managing products.
    /// </summary>
    class ProductService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        public ProductService()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        /// <summary>
        /// Asynchronously gets the list of products.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="FoodModel"/>.</returns>
        public async Task<List<FoodModel>> GetProductsAsync()
        {
            try
            {
                var products = await _httpClient.GetFromJsonAsync<RootApiResponse>("products");
                return products.Results.Select(ConvertToDTO).ToList() ?? new List<FoodModel>();
            }
            catch
            {
                // Handle errors if any
                return new List<FoodModel>();
            }
        }

        /// <summary>
        /// Converts an <see cref="ApiProduct"/> to a <see cref="FoodModel"/>.
        /// </summary>
        /// <param name="apiProduct">The API product to convert.</param>
        /// <returns>The converted <see cref="FoodModel"/>.</returns>
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

        /// <summary>
        /// Root API response class.
        /// </summary>
        public class RootApiResponse
        {
            /// <summary>
            /// Gets or sets the list of API products.
            /// </summary>
            [JsonPropertyName("results")]
            public List<ApiProduct> Results { get; set; }
        }
    }
}
