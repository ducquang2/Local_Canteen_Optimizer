using Local_Canteen_Optimizer.Model;
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
        /// <summary>
        /// Displays a success message dialog.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="xamlRoot">The XAML root of the current window.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ShowSuccessMessage(string message, Microsoft.UI.Xaml.XamlRoot xamlRoot)
        {
            await ShowMessage("Success", message, xamlRoot, "OK", ContentDialogButton.Close, "ms-appx:///Images/success.png");
        }

        /// <summary>
        /// Displays an error message dialog.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="xamlRoot">The XAML root of the current window.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ShowErrorMessage(string message, Microsoft.UI.Xaml.XamlRoot xamlRoot)
        {
            await ShowMessage("Error", message, xamlRoot, "OK", ContentDialogButton.Close, "ms-appx:///Images/error.png");
        }

        /// <summary>
        /// Displays a message dialog with a custom title, message, and image.
        /// </summary>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="xamlRoot">The XAML root of the current window.</param>
        /// <param name="closeButtonText">The text for the close button.</param>
        /// <param name="defaultButton">The default button for the dialog.</param>
        /// <param name="imagePath">The path to the image to display in the dialog.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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

        /// <summary>
        /// Displays a confirmation dialog with a custom message and title.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="xamlRoot">The XAML root of the current window.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the primary button was clicked.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the xamlRoot is null.</exception>
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
