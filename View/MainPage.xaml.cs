using Microsoft.UI.Xaml.Controls;
using Local_Canteen_Optimizer.ViewModel;
using Microsoft.UI.Xaml;
using System;
using Local_Canteen_Optimizer.DAO.AuthenDAO;
using Local_Canteen_Optimizer.Model;
using Windows.Storage;
using System.Text.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly IAuthenDAO _authenDAO;

        /// <summary>
        /// Initializes a new instance of the MainPage class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = new NavigationViewModel();

            _authenDAO = new AuthenDAOImp();

            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("userInfo"))
            {
                var userInfoJson = localSettings.Values["userInfo"] as string;
                var userInfo = JsonSerializer.Deserialize<UserModel>(userInfoJson);

                if (userInfo != null && (userInfo.Role == "admin" || userInfo.Role == "manage"))
                {
                    // User is admin or manage, show the ManageUser button
                    ManageUserButton.Visibility = Visibility.Visible;
                }
                else
                {
                    // User is not admin or manage, hide the ManageUser button
                    ManageUserButton.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ManageUserButton.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Handles the click event of the Logout button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (_authenDAO.LogoutAsync())
            {
                // Handle successful logout, e.g., navigate to login page
                App.m_window.NavigateToAuthPage();
            }
            else
            {
                // Handle logout failure
            }
        }
    }
}
