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
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace WinCord
{
    public sealed partial class MainViewPage : Page
    {
        public static User CurrentUser { get; private set; }

        public MainViewPage()
        {
            this.InitializeComponent();
        }

        private void MessageViewPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MessageViewPage));
        }

    }
}
