using Microsoft.UI.Xaml.Controls;
using Local_Canteen_Optimizer.ViewModel;
using Microsoft.UI.Xaml.Navigation; // Add this line

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AuthPage : Page
    {
        public AuthPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is AuthViewModel authViewModel)
            {
                this.DataContext = authViewModel;
            }
        }
    }
}
