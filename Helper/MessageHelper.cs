using Microsoft.UI.Xaml.Controls;
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
            await ShowMessage("Success", message, xamlRoot, "OK", ContentDialogButton.Close);
        }

        public static async Task ShowErrorMessage(string message, Microsoft.UI.Xaml.XamlRoot xamlRoot)
        {
            await ShowMessage("Error", message, xamlRoot, "OK", ContentDialogButton.Close);
        }

        private static async Task ShowMessage(string title, string message, Microsoft.UI.Xaml.XamlRoot xamlRoot,
            string closeButtonText, ContentDialogButton defaultButton)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = closeButtonText,
                DefaultButton = defaultButton,
                XamlRoot = xamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}
