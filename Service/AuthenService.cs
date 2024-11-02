using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Local_Canteen_Optimizer.Model;
using System.Net.Http;
using System.Net.Http.Json;
using Windows.Storage;

namespace Local_Canteen_Optimizer.Service
{
    class AuthenService
    {
        private readonly HttpClient _httpClient;

        public AuthenService()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        // Phương thức GET để lấy danh sách người dùng
        public async Task<ApiAuthen> LoginAsync(string username, string password)
        {
            try
            {
                var loginData = new { username = username, password = password };
                var apiAuthenRes = await _httpClient.PostAsJsonAsync("/auth", loginData);

                if (apiAuthenRes.IsSuccessStatusCode)
                {
                    var result = await apiAuthenRes.Content.ReadFromJsonAsync<RootApiResponse>();

                    if (result != null)
                    {
                        var localSettings = ApplicationData.Current.LocalSettings;

                        var userInfo = new ApiAuthen
                        {
                            Token = result.Token,
                            _user = new AuthUser // Initialize _user here
                            {
                                Username = result.User.Username,
                                Full_name = result.User.Full_name,
                                Phone_number = result.User.Phone_number,
                                Role = result.User.Role
                            }
                        };

                        localSettings.Values["userToken"] = result.Token;
                        return userInfo;
                    }
                    else
                    {
                        return new ApiAuthen();
                    }
                }
                else
                {
                    return new ApiAuthen();
                }
            }
            catch
            {
                // Xử lý lỗi nếu có
                return new ApiAuthen();
            }
        }

        //public async Task<bool> LoginAsync(string username, string password)
        //{
        //    var loginData = new { username = username, password = password };
        //    var baseUrl = AppSettings.Instance.BaseUrl;
        //    var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/auth", loginData);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadFromJsonAsync<LoginResult>();
        //        if (result != null)
        //        {
        //            IsLoggedIn = true;
        //            UserName = username;
        //            return true;
        //        }
        //    }

        //    return false;
        //}

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
            public string Token { get; set; }
            public AuthUser User { get; set; }
        }
    }
}
