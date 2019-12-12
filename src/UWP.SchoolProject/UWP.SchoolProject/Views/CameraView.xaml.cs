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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.SchoolProject.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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

       
    }
}
