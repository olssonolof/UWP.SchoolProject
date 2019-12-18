using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP.SchoolProject.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP.SchoolProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

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
            if ((Navigation.SelectedItem is NavigationViewItem item) && Navigation != null)
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
