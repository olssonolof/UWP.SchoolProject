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
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MainFrame.Navigate(typeof(HomePage));
        }

        public void ToggleMenu(object sender, RoutedEventArgs e)
        {
            MenuSplitView.IsPaneOpen = !MenuSplitView.IsPaneOpen;
        }

        public void BackEvent(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
                TitleTextBlock.Text = "Main";
                MainMenuItem.IsSelected = true;
            }
        }
        private void MenuChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainMenuItem.IsSelected)
            {
                BackButton.Visibility = Visibility.Collapsed;
                MainFrame.Navigate(typeof(HomePage));
                TitleTextBlock.Text = "Main";
            }
            if (CameraMenuItem.IsSelected)
            {
                BackButton.Visibility = Visibility.Visible;
                MainFrame.Navigate(typeof(CameraView));
                TitleTextBlock.Text = "Food";
            }
        }

        private void CloseMenu(object sender, PointerRoutedEventArgs e)
        {
            MenuSplitView.IsPaneOpen = false;
        }

        private void OpenMenu(object sender, PointerRoutedEventArgs e)
        {
            if (e.KeyModifiers == Windows.System.VirtualKeyModifiers.Control)
            {
                MenuSplitView.IsPaneOpen = true;
            }
        }

    
    }
}
