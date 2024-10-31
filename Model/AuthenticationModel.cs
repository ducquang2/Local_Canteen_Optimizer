using Local_Canteen_Optimizer.Ultis;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Model
{
    public class AuthenticationModel : INotifyPropertyChanged
    {
        private bool _isLoggedIn;
        private string _userName;
        private static readonly HttpClient _httpClient = new HttpClient();

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var loginData = new { username = username, password = password };
            var baseUrl = AppSettings.Instance.BaseUrl;
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/auth", loginData);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResult>();
                if (result != null)
                {
                    IsLoggedIn = true;
                    UserName = username;
                    return true;
                }
            }

            return false;
        }

        public void Logout()
        {
            IsLoggedIn = false;
            UserName = "";
        }
    }

    public class LoginResult
    {
        public string token { get; set; }
    }
}