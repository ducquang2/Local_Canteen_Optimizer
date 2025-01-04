using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View
{
    public sealed partial class Setting : UserControl
    {
        public Setting()
        {
            this.InitializeComponent();
            LoadCurrentTheme();
        }

        private void LoadCurrentTheme()
        {
            if (App.m_window.Content is FrameworkElement framworkElement)
            {
                var currentTheme = framworkElement.ActualTheme;

                switch (currentTheme)
                {
                    case ElementTheme.Light:
                        ThemeComboBox.SelectedIndex = 0;
                        break;
                    case ElementTheme.Dark:
                        ThemeComboBox.SelectedIndex = 1;
                        break;
                    default:
                        ThemeComboBox.SelectedIndex = 2;
                        break;
                }
            }
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTheme = (ThemeComboBox.SelectedItem as ComboBoxItem)?.Tag.ToString();
            var localSettings = ApplicationData.Current.LocalSettings;

            if (App.m_window.Content is FrameworkElement framworkElement)
            {
                switch (selectedTheme)
                {
                    case "Light":
                        framworkElement.RequestedTheme = ElementTheme.Light;
                        localSettings.Values["AppTheme"] = "Light";
                        break;
                    
                    case "Dark":
                        framworkElement.RequestedTheme = ElementTheme.Dark;
                        localSettings.Values["AppTheme"] = "Dark";
                        break;
                    default:
                        framworkElement.RequestedTheme = ElementTheme.Default;
                        localSettings.Values["AppTheme"] = "Default";
                        break;
                }
            }

        }
    }
}
