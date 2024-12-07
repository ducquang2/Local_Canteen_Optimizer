using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Helper
{
    public static class MessageHelper
    {
        public static async Task ShowSuccessMessage(string message, Microsoft.UI.Xaml.XamlRoot xamlRoot)
        {
            await ShowMessage("Success", message, xamlRoot, "OK", ContentDialogButton.Close, "ms-appx:///Images/success.png");
        }

        public static async Task ShowErrorMessage(string message, Microsoft.UI.Xaml.XamlRoot xamlRoot)
        {
            await ShowMessage("Error", message, xamlRoot, "OK", ContentDialogButton.Close, "ms-appx:///Images/error.png");
        }

        private static async Task ShowMessage(string title, string message, Microsoft.UI.Xaml.XamlRoot xamlRoot,
            string closeButtonText, ContentDialogButton defaultButton, string imagePath)
        {
            var image = new Image
            {
                Source = new BitmapImage(new Uri(imagePath)),
                Width = 50,
                Height = 50,
                Margin = new Microsoft.UI.Xaml.Thickness(0, 0, 0, 10)
            };

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            stackPanel.Children.Add(image);
            stackPanel.Children.Add(new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center
            });

            var dialog = new ContentDialog
            {
                Title = title,
                Content = stackPanel,
                CloseButtonText = closeButtonText,
                DefaultButton = defaultButton,
                XamlRoot = xamlRoot
            };

            await dialog.ShowAsync();
        }

        public static async Task<bool> ShowConfirmationDialog(string message, string title, Microsoft.UI.Xaml.XamlRoot xamlRoot)
        {
            if (xamlRoot == null)
                throw new ArgumentNullException(nameof(xamlRoot), "XamlRoot không được null");

            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Ok",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = xamlRoot
            };

            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }
    }
}
