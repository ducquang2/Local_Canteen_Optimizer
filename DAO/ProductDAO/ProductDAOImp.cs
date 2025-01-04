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

namespace Local_Canteen_Optimizer.DAO.ProductDAO
{
    public class ProductDAOImp : IProductDao
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDAOImp"/> class.
        /// </summary>
        public ProductDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        /// <summary>
        /// Adds a new product asynchronously.
        /// </summary>
        /// <param name="newProduct">The new product to add.</param>
        /// <returns>The added product as a <see cref="FoodModel"/>.</returns>
        public async Task<FoodModel> AddProductAsync(FoodModel newProduct)
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    ApiProduct apiProduct = new ApiProduct
                    {
                        product_name = newProduct.Name,
                        price = newProduct.Price,
                        stock_quantity = newProduct.Quantity,
                        image_url = newProduct.ImageSource
                    };

                    var response = await _httpClient.PostAsJsonAsync("api/v1/products", apiProduct);

                    if (response.IsSuccessStatusCode)
                    {
                        var addedProduct = await response.Content.ReadFromJsonAsync<AddApiResponse>();
                        return ConvertToFoodModel(addedProduct.Product);
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {errorContent}");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("User token not found.");
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a list of products asynchronously.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="rowsPerPage">The number of rows per page.</param>
        /// <param name="keyword">The search keyword.</param>
        /// <param name="nameAscending">Sort order by name.</param>
        /// <param name="minPrice">The minimum price filter.</param>
        /// <param name="maxPrice">The maximum price filter.</param>
        /// <returns>A tuple containing the total number of items and a list of <see cref="FoodModel"/>.</returns>
        public async Task<Tuple<int, List<FoodModel>>> GetProductsAsync(int? page, int? rowsPerPage, string keyword, bool nameAscending, double? minPrice, double? maxPrice)
        {
            try
            {
                var sortOrder = nameAscending ? "asc" : "desc";
                var url = $"api/v1/products?page={page}&pageSize={rowsPerPage}&search={keyword}&sort={sortOrder}";
                if (minPrice.HasValue && maxPrice.HasValue)
                {
                    url += $"&minPrice={minPrice.Value}&maxPrice={maxPrice.Value}";
                }
                var products = await _httpClient.GetFromJsonAsync<GetApiResponse>(url);
                var foodModels = products.Results.Select(ConvertToFoodModel).ToList();
                return new Tuple<int, List<FoodModel>>(products.TotalItems, foodModels);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Removes a product asynchronously.
        /// </summary>
        /// <param name="productID">The ID of the product to remove.</param>
        /// <returns>True if the product was removed successfully, otherwise false.</returns>
        public async Task<bool> RemoveProductAsync(int productID)
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    var response = await _httpClient.DeleteAsync($"api/v1/products/{productID}");

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {errorContent}");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("User token not found.");
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Updates a product asynchronously.
        /// </summary>
        /// <param name="newProduct">The product to update.</param>
        /// <returns>The updated product as a <see cref="FoodModel"/>.</returns>
        public async Task<FoodModel> UpdateProductAsync(FoodModel newProduct)
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    ApiProduct apiProduct = new ApiProduct
                    {
                        product_name = newProduct.Name,
                        price = newProduct.Price,
                        stock_quantity = newProduct.Quantity,
                        image_url = newProduct.ImageSource
                    };

                    var response = await _httpClient.PutAsJsonAsync($"api/v1/products/{newProduct.ProductID}", apiProduct);

                    if (response.IsSuccessStatusCode)
                    {
                        var addedProduct = await response.Content.ReadFromJsonAsync<AddApiResponse>();
                        return ConvertToFoodModel(addedProduct.Product);
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {errorContent}");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("User token not found.");
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts an <see cref="ApiProduct"/> to a <see cref="FoodModel"/>.
        /// </summary>
        /// <param name="apiProduct">The API product to convert.</param>
        /// <returns>The converted <see cref="FoodModel"/>.</returns>
        private FoodModel ConvertToFoodModel(ApiProduct apiProduct)
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
        /// Response class for getting products.
        /// </summary>
        public class GetApiResponse
        {
            [JsonPropertyName("totalItems")]
            public int TotalItems { get; set; }

            [JsonPropertyName("results")]
            public List<ApiProduct> Results { get; set; }
        }

        /// <summary>
        /// Response class for adding a product.
        /// </summary>
        public class AddApiResponse
        {
            [JsonPropertyName("product")]
            public ApiProduct Product { get; set; }
        }
    }
}
