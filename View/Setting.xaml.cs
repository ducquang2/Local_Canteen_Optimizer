using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View
{
    /// <summary>
    /// A UserControl that provides settings functionality.
    /// </summary>
    public sealed partial class Setting : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Setting"/> class.
        /// </summary>
        public Setting()
        {
            this.InitializeComponent();
            LoadCurrentTheme();
        }

        /// <summary>
        /// Loads the current theme and sets the ThemeComboBox selection accordingly.
        /// </summary>
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

        /// <summary>
        /// Handles the SelectionChanged event of the ThemeComboBox control.
        /// Updates the application theme based on the selected item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
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
