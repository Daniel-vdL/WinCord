// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.Json;
using WinCord.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinCord.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            this.InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextbox.Text;

            using var client = new HttpClient();

            var user = new User
            {
                Name = username,
            };

            var userJson = JsonSerializer.Serialize(user);

            var context = new StringContent(userJson, System.Text.Encoding.UTF8, "Application/Json");

            var response = await client.PostAsync("https://localhost:7239/api/Users", context);

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            var answerJson = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var answerUser = JsonSerializer.Deserialize<User>(answerJson, options);

            User.CurrentLoggedInUser  = answerUser;

            this.Frame.Navigate(typeof(MessageViewPage));

        }
    }
}
