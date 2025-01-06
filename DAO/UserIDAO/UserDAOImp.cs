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
using Local_Canteen_Optimizer.View.ManageUser;

namespace Local_Canteen_Optimizer.DAO.UserIDAO
{
    /// <summary>
    /// Implementation of IUserDAO interface.
    /// </summary>
    public class UserDAOImp : IUserDAO
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDAOImp"/> class.
        /// </summary>
        public UserDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        /// <summary>
        /// Gets a list of users asynchronously.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="rowsPerPage">The number of rows per page.</param>
        /// <param name="keyword">The search keyword.</param>
        /// <param name="nameAscending">Sort order by name.</param>
        /// <returns>A tuple containing the total number of items and a list of user models.</returns>
        [ArmDot.Client.VirtualizeCode]
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

        /// <summary>
        /// Gets a user asynchronously by username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user model.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<UserModel> GetUserAsync(string username)
        {
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// Adds a new user asynchronously.
        /// </summary>
        /// <param name="newUser">The new user model.</param>
        /// <returns>The added user model.</returns>
        [ArmDot.Client.VirtualizeCode]
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

        /// <summary>
        /// Updates an existing user asynchronously.
        /// </summary>
        /// <param name="updatedUser">The updated user model.</param>
        /// <returns>The updated user model.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<UserModel> UpdateUserAsync(UserModel updatedUser)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("userToken"))
            {
                string userToken = localSettings.Values["userToken"] as string;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                ApiUser apiUser = new ApiUser
                {
                    username = updatedUser.Username,
                    full_name = updatedUser.Full_name,
                    phone_number = updatedUser.Phone_number,
                    role = updatedUser.Role
                };

                var response = await _httpClient.PutAsJsonAsync($"api/v1/users/{updatedUser.UserID}", apiUser);
                if (response.IsSuccessStatusCode)
                {
                    var editedUser = await response.Content.ReadFromJsonAsync<AddApiResponse>();
                    return ConvertToUserModel(editedUser._apiUser);
                }
                else
                {
                    throw new Exception("Error updating user");
                }
            }
            throw new UnauthorizedAccessException("User not authenticated");
        }

        /// <summary>
        /// Removes a user asynchronously.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns>A boolean indicating success or failure.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<bool> RemoveUserAsync(int userID)
        {
            await Task.CompletedTask;
            return false;
        }

        /// <summary>
        /// Converts an ApiUser to a UserModel.
        /// </summary>
        /// <param name="apiUser">The API user.</param>
        /// <returns>The user model.</returns>
        [ArmDot.Client.VirtualizeCode]
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

        /// <summary>
        /// Response class for getting users.
        /// </summary>
        public class GetApiResponse
        {
            [JsonPropertyName("totalItems")]
            public int TotalItems { get; set; }

            [JsonPropertyName("results")]
            public List<ApiUser> Results { get; set; }
        }

        /// <summary>
        /// Response class for adding a user.
        /// </summary>
        public class AddApiResponse
        {
            [JsonPropertyName("user")]
            public ApiUser _apiUser { get; set; }
        }
    }
}
