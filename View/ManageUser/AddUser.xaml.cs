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
    public sealed partial class AddUser : UserControl
    {
        public event EventHandler<UserModel> SaveRequested;
        public event EventHandler CancelRequested;

        public AddUser()
        {
            this.InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;
            // Reset error messages
            UsernameErrorText.Visibility = Visibility.Collapsed;
            PasswordErrorText.Visibility = Visibility.Collapsed;

            // Validate Username
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                UsernameErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // Validate Password
            if (PasswordTextBox.Password.Length < 8)
            {
                PasswordErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // If there are errors, stop here
            if (hasError) return;

            var selectedRole = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString().ToLower();

            var user = new UserModel
            {
                Username = UsernameTextBox.Text,
                Password = PasswordTextBox.Password,
                Full_name = FullNameTextBox.Text,
                Phone_number = PhoneNumberTextBox.Text,
                Role = selectedRole
            };

            SaveRequested?.Invoke(this, user);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
