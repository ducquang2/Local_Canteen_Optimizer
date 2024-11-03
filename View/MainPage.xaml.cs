using Microsoft.UI.Xaml.Controls;
using Local_Canteen_Optimizer.ViewModel;
using Microsoft.UI.Xaml;
using System;
using Local_Canteen_Optimizer.DAO.AuthenDAO;

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

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = new NavigationViewModel();

            _authenDAO = new AuthenDAOImp();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {

            if (_authenDAO.LogoutAsync())
            {
                // Handle successful logout, e.g., navigate to login page
                ((App)Application.Current).m_window.NavigateToAuthPage();
            }
            else
            {
                // Handle logout failure
            }
        }
    }
}
