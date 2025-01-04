using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Local_Canteen_Optimizer.Commands;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;
using System;
using Local_Canteen_Optimizer.DAO.AuthenDAO;

/// <summary>
/// ViewModel for handling authentication logic.
/// </summary>
namespace Local_Canteen_Optimizer.ViewModel
{
    public class AuthViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private IAuthenDAO _dao = null;
        private string _username;
        private string _password;
        private string _errorMessage;

        /// <summary>
        /// Event to notify successful login.
        /// </summary>
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

        /// <summary>
        /// Command for logging in.
        /// </summary>
        public ICommand LoginCommand { get; }

        /// <summary>
        /// Command for logging out.
        /// </summary>
        public ICommand LogoutCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthViewModel"/> class.
        /// </summary>
        public AuthViewModel()
        {
            _dao = new AuthenDAOImp();
            LoginCommand = new RelayCommand<object>(_ => Login());
            LogoutCommand = new RelayCommand<object>(_ => Logout());
        }

        /// <summary>
        /// Handles the login logic.
        /// </summary>
        private async void Login()
        {
            ErrorMessage = "";
            if (Username == "")
            {
                ErrorMessage = "Username Can't be Empty";
                return;
            }

            if (Password == "")
            {
                ErrorMessage = "Password Can't be Empty";
                return;
            }
            var result = await _dao.LoginAsync(Username, Password);
            if (result.Token != null)
            {
                // Successful login
                App.m_window.NavigateToMainPage();
                LoginSuccess?.Invoke(); // Raise the event
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
            }
        }

        /// <summary>
        /// Handles the logout logic.
        /// </summary>
        private async void Logout()
        {
            _dao.LogoutAsync();
        }

        /// <summary>
        /// Event to notify property changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifies that a property has changed.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
