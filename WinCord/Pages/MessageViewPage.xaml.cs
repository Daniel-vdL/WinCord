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
using System.Threading.Tasks;
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
            _timer.Interval = TimeSpan.FromSeconds(30);
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
           this.LoadMessages();
        }

        private async void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string messageContent = MessageSuggestBox.Text;

            // Check if the message content is not empty
            if (!string.IsNullOrWhiteSpace(messageContent))
            {
                // Check if the message is a command
                if (messageContent.StartsWith("/"))
                {
                    HandleCommand(messageContent);
                }
                else
                {
                    await SendMessage(messageContent);
                }
            }
        }

        private async Task SendMessage(string messageContent)
        {
            using var client = new HttpClient();

            var newMessage = new Message
            {
                Username = User.CurrentLoggedInUser.Name,
                UserId = User.CurrentLoggedInUser.Id,
                Content = messageContent
            };

            var messageJson = JsonSerializer.Serialize(newMessage);

            var context = new StringContent(messageJson, System.Text.Encoding.UTF8, "Application/Json");

            var response = await client.PostAsync("https://localhost:7239/api/Messages", context);

            if (response.IsSuccessStatusCode)
            {
                this.LoadMessages();
                MessageListView.ScrollIntoView(newMessage);
                MessageSuggestBox.Text = "";
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

        private async void HandleCommand(string command)
        {
            if (command.StartsWith("/"))
            {
                if (command.StartsWith("/magic"))
                {
                    string originalContent = command.Substring("/magic".Length).Trim();
                    string transformedContent = PerformMagic(originalContent);
                    string magicMessage = $"[MAGIC] {transformedContent}";
                    await SendMessage(magicMessage);
                }
                else if (command.StartsWith("/reverse"))
                {
                    string reversedContent = ReverseText(command.Substring("/reverse".Length).Trim());
                    await SendMessage(reversedContent);
                }
                else if (command.StartsWith("/emphasize"))
                {
                    string emphasizedContent = command.Substring("/emphasize".Length).Trim();
                    string emphasizedMessage = $"{EmphasizeText(emphasizedContent)}";
                    await SendMessage(emphasizedMessage);
                }
                else
                {
                    // Command not recognized
                    string errorMessage = "Command not recognized. Type / for available commands.";
                    await SendMessage(errorMessage);
                }
            }
            else
            {
                // Regular message, not a command
                await SendMessage(command);
            }
        }

        private string PerformMagic(string text)
        {
            string[] magicTransformations = {
                "Abracadabra!",
                "Presto!",
                "Voila!",
                "Hocus Pocus!",
                "Alakazam!",
                "Shazam!",
                "Wingardium Leviosa!",
                "Expecto Patronum!",
                "Expelliarmus!",
                "Avada Kedavra!",
                "Accio!",
                "Alohomora!",
                "Lumos!",
                "Nox!",
                "Riddikulus!",
            };

            Random random = new Random();
            int index = random.Next(magicTransformations.Length);
            string transformation = magicTransformations[index];

            return $"{transformation} {text}";
        }

        private string ReverseText(string text)
        {
            char[] charArray = text.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private string EmphasizeText(string text)
        {
            string emphasizedText = text.ToUpper() + "!!!";
            return emphasizedText;
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            string userInput = sender.Text;

            // Check if the user input starts with '/'
            if (userInput.StartsWith("/"))
            {
                // Filter suggestions based on user input
                string inputWithoutSlash = userInput.Substring(1);
                IEnumerable<string> filteredSuggestions = GetCommandSuggestions().Where(s => s.StartsWith(inputWithoutSlash));
                sender.ItemsSource = filteredSuggestions;
            }
            else
            {
                // If the user doesn't input '/', clear the suggestion list
                sender.ItemsSource = null;
            }
        }

        private List<string> GetCommandSuggestions()
        {
            return new List<string> { "/magic", "/reverse", "/emphasize" };
        }
    }
}
