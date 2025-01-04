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

        /// <summary>
        /// Event triggered when a request to add a user is made.
        /// </summary>
        public event EventHandler AddUserRequested;

        /// <summary>
        /// Event triggered when a request to edit a user is made.
        /// </summary>
        public event EventHandler<UserModel> EditUserRequested;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListUser"/> class.
        /// </summary>
        public ListUser()
        {
            this.InitializeComponent();
            InitializeAsync();
        }

        /// <summary>
        /// Initializes the view model asynchronously.
        /// </summary>
        public async Task InitializeAsync()
        {
            manageUserViewModel = new ManageUserViewModel();
            await manageUserViewModel.Init();
            UpdatePagingInfo_bootstrap();
        }

        /// <summary>
        /// Updates the paging information for the bootstrap.
        /// </summary>
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

        /// <summary>
        /// Handles the selection changed event of the pages combo box.
        /// </summary>
        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic item = pagesComboBox.SelectedItem;

            if (item != null)
            {
                manageUserViewModel.Load(item.Page);
            }
        }

        /// <summary>
        /// Handles the text changed event of the keyword text box.
        /// </summary>
        private void keywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Handles the click event of the search button.
        /// </summary>
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Handles the click event of the add user button.
        /// </summary>
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            AddUserRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the click event of the edit button.
        /// </summary>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button).Tag as UserModel;
            EditUserRequested?.Invoke(this, user);
        }

        /// <summary>
        /// Adds a user to the view model.
        /// </summary>
        /// <param name="user">The user to add.</param>
        public void AddUser(UserModel user)
        {
            manageUserViewModel.AddUser(user);
        }

        /// <summary>
        /// Edits a user in the view model.
        /// </summary>
        /// <param name="user">The user to edit.</param>
        public void EditUser(UserModel user)
        {
            manageUserViewModel.EditUser(user);
        }
    }
}
