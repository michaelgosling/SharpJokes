﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SharpJokes {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page {
        public string Version { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }

        public AboutPage() {
            this.InitializeComponent();
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Assembly asm = Assembly.GetCallingAssembly();
            var descriptionAttribute = asm.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false).OfType<AssemblyDescriptionAttribute>().FirstOrDefault();
            var versionAttribute = asm.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false).OfType<AssemblyFileVersionAttribute>().FirstOrDefault();
            var companyAttribute = asm.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false).OfType<AssemblyCompanyAttribute>().FirstOrDefault();

            this.Description = descriptionAttribute.Description;
            this.Company = companyAttribute.Company;
            this.Version = versionAttribute.Version;
        }

        private async void GoToReddit_Click(object sender, RoutedEventArgs e) {
            string uriToLaunch = @"http://www.reddit.com/r/ProgrammerHumor/";
            var uri = new Uri(uriToLaunch);

            var success = await Windows.System.Launcher.LaunchUriAsync(uri);

            if (success) {
                
            }
            else {
                
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Exit();
        }
    }
}
