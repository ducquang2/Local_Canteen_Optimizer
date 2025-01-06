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

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageUser"/> class.
        /// </summary>
        public ManageUser()
        {
            this.InitializeComponent();

            // Initialize user list
            userListControl = new ListUser();
            userListControl.AddUserRequested += OnAddUserRequested;
            userListControl.EditUserRequested += OnEditUserRequested;

            // Initialize add user form
            addUserControl = new AddUser();
            addUserControl.SaveRequested += OnAddSaveRequested;
            addUserControl.CancelRequested += OnCancelRequested;

            // Initialize edit user form
            editUserControl = new EditUser();
            editUserControl.SaveRequested += OnEditSaveRequested;
            editUserControl.CancelRequested += OnCancelRequested;

            // Display user list
            ManageUserContent.Content = userListControl;
        }

        /// <summary>
        /// Handles the AddUserRequested event of the userListControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnAddUserRequested(object sender, EventArgs e)
        {
            ManageUserContent.Content = addUserControl;
        }

        /// <summary>
        /// Handles the EditUserRequested event of the userListControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="user">The user to be edited.</param>
        private void OnEditUserRequested(object sender, UserModel user)
        {
            editUserControl.SetUser(user);
            ManageUserContent.Content = editUserControl;
        }

        /// <summary>
        /// Handles the SaveRequested event of the addUserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="user">The user to be added.</param>
        private void OnAddSaveRequested(object sender, UserModel user)
        {
            userListControl.AddUser(user);
            ManageUserContent.Content = userListControl;
        }

        /// <summary>
        /// Handles the SaveRequested event of the editUserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="user">The user to be edited.</param>
        private void OnEditSaveRequested(object sender, UserModel user)
        {
            userListControl.EditUser(user);
            ManageUserContent.Content = userListControl;
        }

        /// <summary>
        /// Handles the CancelRequested event of the addUserControl and editUserControl controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnCancelRequested(object sender, EventArgs e)
        {
            ManageUserContent.Content = userListControl;
        }
    }
}
