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

        public ProductDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        // Phương thức POST để thêm một món ăn mới
        public async Task<FoodModel> AddProductAsync(FoodModel newProduct)
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

                    ApiProduct apiProduct = new ApiProduct
                    {
                        product_name = newProduct.Name,
                        price = newProduct.Price,
                        stock_quantity = newProduct.Quantity,
                        image_url = newProduct.ImageSource
                    };

                    var response = await _httpClient.PostAsJsonAsync("api/v1/products/add", apiProduct);

                    if (response.IsSuccessStatusCode)
                    {
                        var addedProduct = await response.Content.ReadFromJsonAsync<AddApiResponse>();
                        return ConvertToFoodModel(addedProduct.Product);
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

        // Phương thức GET để lấy danh sách người dùng
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
                // Xử lý lỗi nếu có
                return null;
            }
        }

        // Delete product
        public async Task<bool> RemoveProductAsync(int productID)
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    //string userToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImFkbWluMDEiLCJyb2xlIjoiYWRtaW4iLCJpYXQiOjE3MzA2MDc1NjcsImV4cCI6MTczMDYxODM2N30.yeQlwaObIRDsJxUo67AexY8nx2ynSBXVNU5zWfwR8Mg";
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    var response = await _httpClient.GetAsync($"api/v1/products/delete/{productID}");

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

        public async Task<FoodModel> UpdateProductAsync(FoodModel newProduct)
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    //string userToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImFkbWluMDEiLCJyb2xlIjoiYWRtaW4iLCJpYXQiOjE3MzA1OTU5OTcsImV4cCI6MTczMDYwNjc5N30.JDixEiIgduUJ9wUXJaoDXyYX7c654W6VwKxwAtQOJaw";
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    ApiProduct apiProduct = new ApiProduct
                    {
                        product_name = newProduct.Name,
                        price = newProduct.Price,
                        stock_quantity = newProduct.Quantity,
                        image_url = newProduct.ImageSource
                    };

                    var response = await _httpClient.PostAsJsonAsync($"api/v1/products/update/{newProduct.ProductID}", apiProduct);

                    if (response.IsSuccessStatusCode)
                    {
                        var addedProduct = await response.Content.ReadFromJsonAsync<AddApiResponse>(); // add api response same structure with update api response
                        return ConvertToFoodModel(addedProduct.Product);
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

        public class GetApiResponse
        {
            [JsonPropertyName("totalItems")]
            public int TotalItems { get; set; }

            [JsonPropertyName("results")]
            public List<ApiProduct> Results { get; set; }
        }

        public class AddApiResponse
        {
            [JsonPropertyName("product")]
            public ApiProduct Product { get; set; }
        }
    }
}
