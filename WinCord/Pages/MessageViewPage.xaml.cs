// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

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
using WinCord.Models;
using WinCord.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace WinCord
{
    public sealed partial class MessageViewPage : Page
    {
        private DispatcherTimer _timer;

        public MessageViewPage()
        {
            this.InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(10);
            _timer.Tick += Timer_Tick;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.LoadMessages();
            _timer.Start(); // Start the timer when the page is navigated to
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            _timer.Stop(); // Stop the timer when navigating away from the page
        }

        private async void LoadMessages()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7239/api/Messages");
            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var messages = JsonSerializer.Deserialize<List<Message>>(content, options);

            MessageListView.ItemsSource = messages;

            if (MessageListView.Items.Count > 0)
            {
                var lastIndex = MessageListView.Items.Count - 1;
                MessageListView.ScrollIntoView(MessageListView.Items[lastIndex]);
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegistrationPage));
        }

        private void Timer_Tick(object sender, object e)
        {
           this.LoadMessages(); // Reload messages every 5 seconds
        }
    }
}
