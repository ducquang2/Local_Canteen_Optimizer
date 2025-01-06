using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.Service;
using Windows.Storage;
using System.Text.Json;

namespace Local_Canteen_Optimizer.DAO.AuthenDAO
{
    /// <summary>
    /// Implementation of the authentication data access object.
    /// </summary>
    public class AuthenDAOImp : IAuthenDAO
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenDAOImp"/> class.
        /// </summary>
        public AuthenDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        /// <summary>
        /// Logs in a user asynchronously.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A task that represents the asynchronous login operation. The task result contains the authentication model.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<AuthenModel> LoginAsync(string username, string password)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var loginData = new { username = username, password = password };
            var apiAuthenRes = await _httpClient.PostAsJsonAsync("/api/v1/auth", loginData);

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
                    localSettings.Values["userInfo"] = JsonSerializer.Serialize(userInfo._user);
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

        /// <summary>
        /// Logs out a user asynchronously.
        /// </summary>
        /// <returns>True if the user was logged out successfully; otherwise, false.</returns>
        [ArmDot.Client.VirtualizeCode]
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

    /// <summary>
    /// Represents the response from the login API.
    /// </summary>
    public class LoginApiResponse
    {
        /// <summary>
        /// Gets or sets the authentication token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the user information.
        /// </summary>
        public UserModel User { get; set; }
    }
}
