using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.Service;
using Windows.Storage;

namespace Local_Canteen_Optimizer.DAO.AuthenDAO
{
    public class AuthenDAOImp : IAuthenDAO
    {
        private readonly HttpClient _httpClient;

        public AuthenDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        public async Task<AuthenModel> LoginAsync(string username, string password) 
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var loginData = new { username = username, password = password };
            var apiAuthenRes = await _httpClient.PostAsJsonAsync("/auth", loginData);

            if (apiAuthenRes.IsSuccessStatusCode)
            {
                var result = await apiAuthenRes.Content.ReadFromJsonAsync<LoginApiResponse>();

                if (result != null)
                {
                    var userInfo = new AuthenModel
                    {
                        Token = result.Token,
                        _user = new UserModel // Initialize _user here
                        {
                            Username = result.User.Username,
                            Full_name = result.User.Full_name,
                            Phone_number = result.User.Phone_number,
                            Role = result.User.Role
                        }
                    };

                    localSettings.Values["userToken"] = result.Token;
                    localSettings.Values["userInfo"] = userInfo._user;
                    return userInfo;
                }
                else
                {
                    return new AuthenModel();
                }
            }
            else
            {
                return new AuthenModel();
            }
        }
        
        public bool LogoutAsync()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("userToken"))
            {
                localSettings.Values.Remove("userToken");
                localSettings.Values.Remove("userInfo");
                return true;
            }
            else
            {
                Console.WriteLine("User not logged in");
                return false;
            }
        }
    }

    public class LoginApiResponse
    {
        public string Token { get; set; }
        public UserModel User { get; set; }
    }
}
