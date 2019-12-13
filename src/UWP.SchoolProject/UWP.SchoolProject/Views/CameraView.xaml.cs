using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using UWP.SchoolProject.ViewModels;


namespace UWP.SchoolProject.Views
{

    public sealed partial class CameraView : Page
    {
        private CameraViewModel ViewModel;
        public CameraView()
        {
            this.InitializeComponent();
            this.ViewModel = new CameraViewModel();
        }


        public async void TakePhoto(object sender, RoutedEventArgs e)
        {
            PhotoButton.IsEnabled = false;
            var takePicture =  ViewModel.GetImageInfo();

            await takePicture;
            PhotoButton.IsEnabled = true;
        }

        private async void OpenPhoto_ClickAsync(object sender, RoutedEventArgs e)
        {
            OpenPhoto.IsEnabled = false;
            App.Key = App.Key ?? await ViewModel.OpenKeyWindow();
            if (string.IsNullOrWhiteSpace(App.Key))
            {
                OpenPhoto.IsEnabled = true;
                return;
            }
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

           StorageFile file = await picker.PickSingleFileAsync();

            await ViewModel.GetImageInfo(file);
            OpenPhoto.IsEnabled = true;

        }

        private async void SaveImage(object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveImageToDisc();
        }
    }
}
