using Local_Canteen_Optimizer.Model;
using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View.ManageUser
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManageUser : UserControl
    {
        private ListUser userListControl;
        private AddUser addUserControl;
        private EditUser editUserControl;

        public ManageUser()
        {
            this.InitializeComponent();

            // Khởi tạo danh sách user
            userListControl = new ListUser();
            userListControl.AddUserRequested += OnAddUserRequested;
            userListControl.EditUserRequested += OnEditUserRequested;

            // Initialize add user form
            addUserControl = new AddUser();
            addUserControl.SaveRequested += OnAddSaveRequested;
            addUserControl.CancelRequested += OnCancelRequested;

            // Edit User
            editUserControl = new EditUser();
            editUserControl.SaveRequested += OnEditSaveRequested;
            editUserControl.CancelRequested += OnCancelRequested;

            // Hiển thị danh sách
            ManageUserContent.Content = userListControl;
        }

        private void OnAddUserRequested(object sender, EventArgs e)
        {
            ManageUserContent.Content = addUserControl;
        }

        private void OnEditUserRequested(object sender, UserModel user)
        {
            editUserControl.SetUser(user);
            ManageUserContent.Content = editUserControl;
        }

        private void OnAddSaveRequested(object sender, UserModel user)
        {
            userListControl.AddUser(user);
            ManageUserContent.Content = userListControl;
        }

        private void OnEditSaveRequested(object sender, UserModel user)
        {
            userListControl.EditUser(user);
            ManageUserContent.Content = userListControl;
        }

        private void OnCancelRequested(object sender, EventArgs e)
        {
            ManageUserContent.Content = userListControl;
        }
    }
}
