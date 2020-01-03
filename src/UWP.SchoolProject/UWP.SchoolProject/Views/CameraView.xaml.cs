using System;
using UWP.SchoolProject.Services;
using UWP.SchoolProject.ViewModels;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace UWP.SchoolProject.Views
{

    public sealed partial class CameraView : Page
    {
        private CameraViewModel viewModel;
        public CameraView()
        {
            this.InitializeComponent();
            this.viewModel = new CameraViewModel();
        }


        public async void TakePhoto(object sender, RoutedEventArgs e)
        {
            PhotoButton.IsEnabled = false;
            var takePicture = viewModel.GetImageInfo();

            await takePicture;
            PhotoButton.IsEnabled = true;
        }

        private async void OpenPhoto_ClickAsync(object sender, RoutedEventArgs e)
        {
            OpenPhoto.IsEnabled = false;
            // App.Key = App.Key ?? await viewModel.OpenKeyWindow();

            await CkeckForKey.CheckIfKeyExist();

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

            await viewModel.GetImageInfo(file);
            OpenPhoto.IsEnabled = true;

        }

        private async void SaveImage(object sender, RoutedEventArgs e)
        {
            await viewModel.SaveImageToDisc();
        }
    }
}
