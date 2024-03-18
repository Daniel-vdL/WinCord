using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinCord.Models;
using System.Security.Cryptography;

namespace WinCord.Pages
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7239/api/Users");
            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var users = JsonSerializer.Deserialize<List<User>>(content, options);

            var username = UsernameTextbox.Text;
            var password = PasswordTextbox.Text;

            foreach ( var user in users )
            {
                if (username == user.Name && VerifyPassword(password, user.Password))
                {
                    User.CurrentLoggedInUser = user;

                    this.Frame.Navigate(typeof(MessageViewPage));
                }
                else
                {
                    ContentDialog ErrorDialog = new ContentDialog
                    {
                        Title = "Login failed!",
                        Content = "Click 'Ok' to continue",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot,
                    };

                    ContentDialogResult result = await ErrorDialog.ShowAsync();
                }
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return SecureHasher.Verify(password, hashedPassword);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegistrationPage));
        }
    }
}
