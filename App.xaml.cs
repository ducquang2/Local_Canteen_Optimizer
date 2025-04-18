﻿using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using Windows.Storage;
using System.Text.Json;
using System.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            //ApplySavedTheme();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            //var root = new Frame();
            //m_window.Content = root;
            //var name = "Local_Canteen_Optimizer.MainPage";
            //var type = Type.GetType(name);
            //root.Navigate(type);

            m_window.Activate();
            //m_window.AppWindow.Resize(new Windows.Graphics.SizeInt32(1024, 768));

            if (IsUserAuthenticated())
            {
                m_window.NavigateToMainPage();
            }
            else
            {
                m_window.NavigateToAuthPage();
            }
        }

        /// <summary>
        /// Gets the main window of the application.
        /// </summary>
        public static MainWindow m_window { get; private set; }

        /// <summary>
        /// Checks if the user is authenticated.
        /// </summary>
        /// <returns>True if the user is authenticated, otherwise false.</returns>
        private bool IsUserAuthenticated()
        {
            // Implement your authentication check logic here
            // Need to check if token is expired
            var localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey("userToken"))
            {
                string userToken = localSettings.Values["userToken"] as string;

                if (IsTokenExpired(userToken))
                {
                    localSettings.Values.Remove("userToken");
                    Console.WriteLine("User token is expired and has been removed.");
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the token is expired.
        /// </summary>
        /// <param name="token">The token to check.</param>
        /// <returns>True if the token is expired, otherwise false.</returns>
        private bool IsTokenExpired(string token)
        {
            try
            {
                var tokenParts = token.Split('.');
                if (tokenParts.Length != 3)
                {
                    return true; // Invalid token format
                }

                var payload = tokenParts[1];
                var jsonBytes = Convert.FromBase64String(PadBase64String(payload));
                var jsonString = Encoding.UTF8.GetString(jsonBytes);
                var tokenPayload = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);

                if (tokenPayload != null && tokenPayload.TryGetValue("exp", out var exp))
                {
                    var expirationTime = DateTimeOffset.FromUnixTimeSeconds(exp.GetInt64());
                    return expirationTime < DateTimeOffset.UtcNow;
                }

                return true; // If we can't parse the expiration time, assume the token is expired
            }
            catch
            {
                return true; // If any error occurs, assume the token is expired
            }
        }

        /// <summary>
        /// Pads a Base64 string to ensure it is properly formatted.
        /// </summary>
        /// <param name="base64">The Base64 string to pad.</param>
        /// <returns>The padded Base64 string.</returns>
        private string PadBase64String(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: return base64 + "==";
                case 3: return base64 + "=";
                default: return base64;
            }
        }

        /// <summary>
        /// Applies the saved theme to the application.
        /// </summary>
        private void ApplySavedTheme()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var savedTheme = localSettings.Values["AppTheme"] as string;

            if (savedTheme != null)
            {
                switch (savedTheme)
                {
                    case "Light":
                        ((FrameworkElement)App.m_window.Content).RequestedTheme = ElementTheme.Light;
                        break;
                    case "Dark":
                        ((FrameworkElement)App.m_window.Content).RequestedTheme = ElementTheme.Dark;
                        break;
                    default:
                        ((FrameworkElement)App.m_window.Content).RequestedTheme = ElementTheme.Default;
                        break;
                }
            }
        }
    }
}
