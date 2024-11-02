using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
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
                var products = await _httpClient.GetFromJsonAsync<RootApiResponse>("/products");
                return products.Results ?? new List<FoodModel>();
            }
            catch
            {
                // Xử lý lỗi nếu có
                return new List<FoodModel>();
            }
        }

        //public static UserDTO ConvertToDTO(ApiUser apiUser)
        //{
        //    return new UserDTO
        //    {
        //        Gender = apiUser.Gender,
        //        FullName = $"{apiUser.Name.Title} {apiUser.Name.First} {apiUser.Name.Last}",
        //        Location = $"{apiUser.Location.Street.Number} {apiUser.Location.Street.Name}, {apiUser.Location.City}, {apiUser.Location.State}, {apiUser.Location.Country}, {apiUser.Location.Postcode}"
        //    };
        //}

        public class RootApiResponse
        {
            public List<FoodModel> Results { get; set; }
        }
    }
}
