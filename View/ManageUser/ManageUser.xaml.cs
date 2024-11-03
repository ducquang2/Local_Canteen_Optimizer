using Microsoft.UI.Xaml.Controls;

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

        public ManageUser()
        {
            this.InitializeComponent();

            // Khởi tạo danh sách user
            userListControl = new ListUser();

            // Hiển thị danh sách
            ManageUserContent.Content = userListControl;
        }
    }
}
