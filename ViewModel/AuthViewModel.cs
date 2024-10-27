using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.View;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Local_Canteen_Optimizer.Commands;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;
using System;

namespace Local_Canteen_Optimizer.ViewModel
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        private AuthenticationModel _authenticationModel = new AuthenticationModel();
        private string _username;
        private string _password;
        private string _errorMessage;

        public event Action LoginSuccess; // Event to notify successful login

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoggedIn => _authenticationModel.IsLoggedIn;
        public string UserName => _authenticationModel.UserName;

        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }

        public AuthViewModel()
        {
            LoginCommand = new RelayCommand<object>(_ => Login());
            LogoutCommand = new RelayCommand<object>(_ => Logout());
        }

        private async void Login()
        {
            ErrorMessage = "";
            if (await _authenticationModel.LoginAsync(Username, Password))
            {
                // Successful login
                LoginSuccess?.Invoke(); // Raise the event
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
            }
        }

        private void Logout()
        {
            _authenticationModel.Logout();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
