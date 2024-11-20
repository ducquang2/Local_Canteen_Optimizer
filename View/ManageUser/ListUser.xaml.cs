using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View.ManageUser
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListUser : UserControl
    {
        ManageUserViewModel manageUserViewModel;
        public event EventHandler AddUserRequested;
        public event EventHandler<UserModel> EditUserRequested;
        public ListUser()
        {
            this.InitializeComponent();
            InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            manageUserViewModel = new ManageUserViewModel();
            await manageUserViewModel.Init();
            UpdatePagingInfo_bootstrap();
        }

        void UpdatePagingInfo_bootstrap()
        {
            var infoList = new List<object>();
            for (int i = 1; i <= manageUserViewModel.TotalPages; i++)
            {
                infoList.Add(new
                {
                    Page = i,
                    Total = manageUserViewModel.TotalPages
                });
            };

            pagesComboBox.ItemsSource = infoList;
            pagesComboBox.SelectedIndex = 0;
        }

        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic item = pagesComboBox.SelectedItem;

            if (item != null)
            {
                manageUserViewModel.Load(item.Page);
            }
        }

        private void keywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            AddUserRequested?.Invoke(this, EventArgs.Empty);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button).Tag as UserModel;
            EditUserRequested?.Invoke(this, user);
        }

        public void AddUser(UserModel user)
        {
            manageUserViewModel.AddUser(user);
        }
    }
}
