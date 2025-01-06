using Microsoft.UI.Xaml.Controls;
using Local_Canteen_Optimizer.ViewModel;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AuthPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthPage"/> class.
        /// </summary>
        public AuthPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Called when the page is navigated to.
        /// </summary>
        /// <param name="e">The event data that describes how this page was reached.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is AuthViewModel authViewModel)
            {
                this.DataContext = authViewModel;
            }
            base.OnNavigatedTo(e);
        }
    }
}
