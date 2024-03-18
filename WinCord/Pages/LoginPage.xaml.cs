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

            var authenticatedUser = users.FirstOrDefault(u => u.Name == username && u.Password == password);

            if (authenticatedUser != null)
            {
                User.CurrentLoggedInUser = authenticatedUser;

                this.Frame.Navigate(typeof(MessageViewPage));
            }
            else
            {
                ContentDialog ErrorDialog = new ContentDialog
                {
                    Title = "Message creation failed!",
                    Content = "Click 'Ok' to continue",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot,
                };

                ContentDialogResult result = await ErrorDialog.ShowAsync();
            }
        }
    }
}
