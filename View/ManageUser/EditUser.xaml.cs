using System;
using Microsoft.UI.Xaml;
using Local_Canteen_Optimizer.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View.ManageUser
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditUser : UserControl
    {
        public event EventHandler<UserModel> SaveRequested;
        public event EventHandler CancelRequested;
        private UserModel currentUser;

        public EditUser()
        {
            this.InitializeComponent();
        }

        public void SetUser(UserModel user)
        {
            currentUser = user;
            IdTextBox.Text = user.UserID;
            UsernameTextBox.Text = user.Username;
            PasswordTextBox.Text = user.Password;
            FullNameTextBox.Text = user.Full_name;
            PhoneNumberTextBox.Text = user.Phone_number;
            RoleComboBox.SelectedItem = user.Role.ToUpper();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;

            // Reset error messages
            UsernameErrorText.Visibility = Visibility.Collapsed;
            PasswordErrorText.Visibility = Visibility.Collapsed;
            FullNameErrorText.Visibility = Visibility.Collapsed;
            PhoneNumberErrorText.Visibility = Visibility.Collapsed;
            RoleErrorText.Visibility = Visibility.Collapsed;

            // Validate Username
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                UsernameErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // Validate Password
            if (!string.IsNullOrWhiteSpace(PasswordTextBox.Text) && PasswordTextBox.Text.Length < 8)
            {
                PasswordErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // If there are errors, stop here
            if (hasError) return;

            currentUser.UserID = IdTextBox.Text;
            currentUser.Username = UsernameTextBox.Text;
            currentUser.Password = PasswordTextBox.Text;
            currentUser.Full_name = FullNameTextBox.Text;
            currentUser.Phone_number = PhoneNumberTextBox.Text;
            currentUser.Role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString().ToLower();

            SaveRequested?.Invoke(this, currentUser);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset error messages
            UsernameErrorText.Visibility = Visibility.Collapsed;
            PasswordErrorText.Visibility = Visibility.Collapsed;
            FullNameErrorText.Visibility = Visibility.Collapsed;
            PhoneNumberErrorText.Visibility = Visibility.Collapsed;
            RoleErrorText.Visibility = Visibility.Collapsed;

            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
