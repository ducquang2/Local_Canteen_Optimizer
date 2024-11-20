using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Local_Canteen_Optimizer.Service;
using System.Text.Json.Serialization;
using Windows.Storage;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Local_Canteen_Optimizer.DAO.UserIDAO
{
    public class UserDAOImp : IUserDAO
    {
        private readonly HttpClient _httpClient;

        public UserDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        public async Task<Tuple<int, List<UserModel>>> GetUsersAsync(int? page, int? rowsPerPage, string keyword, bool nameAscending)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey("userToken"))
            {
                try
                    {
                    string userToken = localSettings.Values["userToken"] as string;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    var sortOrder = nameAscending ? "asc" : "desc";
                    var users = await _httpClient.GetFromJsonAsync<GetApiResponse>($"api/v1/users?page={page}&pageSize={rowsPerPage}&search={keyword}&sort={sortOrder}");
                    var userModels = users.Results.Select(ConvertToUserModel).ToList();
                    return new Tuple<int, List<UserModel>>(users.TotalItems, userModels);
                }
                catch
                {
                    // Handle error if any
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

        public async Task<UserModel> GetUserAsync(string username)
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<UserModel> AddUserAsync(UserModel newUser)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("userToken"))
            {
                string userToken = localSettings.Values["userToken"] as string;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                var response = await _httpClient.PostAsJsonAsync("api/v1/users", newUser);
                if (response.IsSuccessStatusCode)
                {
                    var addedUser = await response.Content.ReadFromJsonAsync<AddApiResponse>();
                    return ConvertToUserModel(addedUser._apiUser);
                }
                else
                {
                    throw new Exception("Error adding user");
                }
            }
            throw new UnauthorizedAccessException("User not authenticated");
        }

        public async Task<UserModel> UpdateUserAsync(UserModel updatedUser)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("userToken"))
            {
                string userToken = localSettings.Values["userToken"] as string;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                var response = await _httpClient.PutAsJsonAsync($"api/v1/users/{updatedUser.Username}", updatedUser);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserModel>();
                }
                else
                {
                    throw new Exception("Error updating user");
                }
            }
            throw new UnauthorizedAccessException("User not authenticated");
        }

        public async Task<bool> RemoveUserAsync(int userID)
        {
            await Task.CompletedTask;
            return false;
        }

        private UserModel ConvertToUserModel(ApiUser apiUser)
        {
            return new UserModel
            {
                UserID = apiUser.user_id.ToString(),
                Username = apiUser.username,
                Full_name = apiUser.full_name,
                Phone_number = apiUser.phone_number,
                Role = apiUser.role
            };
        }

        public class GetApiResponse
        {
            [JsonPropertyName("totalItems")]
            public int TotalItems { get; set; }

            [JsonPropertyName("results")]
            public List<ApiUser> Results { get; set; }
        }

        public class AddApiResponse
        {
            [JsonPropertyName("user")]
            public ApiUser _apiUser { get; set; }
        }
    }
}
