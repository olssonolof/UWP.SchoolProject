﻿using UWP.SchoolProject.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP.SchoolProject
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }



        private void Navigation_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(HomePage));
        }



        private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                MainFrame.Navigate(typeof(Settings));
            }
            else if ((Navigation.SelectedItem is NavigationViewItem item) && Navigation != null)
            {

                switch (item.Tag)
                {
                    case "Home":
                        MainFrame.Navigate(typeof(HomePage));
                        break;

                    case "Camera":
                        MainFrame.Navigate(typeof(CameraView));
                        break;


                }
            }


            Navigation.IsBackEnabled = true;
        }
    }
}
