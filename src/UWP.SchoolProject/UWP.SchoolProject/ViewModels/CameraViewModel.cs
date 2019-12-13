using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using ImageAnalyze.Models;

namespace UWP.SchoolProject.ViewModels
{
    class CameraViewModel : INotifyPropertyChanged
    {
        private ImageSource image;

        public ImageSource Image
        {
            get => image;
            set
            {
                image = value;
                NotifyPropertyChanged();
            }
        }


        public ObservableCollection<AiAnswer> ApiAnswer { get; set; }



        private string description;

        public string Description
        {
            get => description;
            set
            {
                description = value;
                NotifyPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CameraViewModel()
        {
            ApiAnswer = new ObservableCollection<AiAnswer>();
        }


        public async Task GetImageInfo(StorageFile picFromDisc = null)
        {

            App.Key = App.Key ?? await OpenKeyWindow();
            if (string.IsNullOrWhiteSpace(App.Key))
            {
                return;
            }
            StorageFile photo;

            if (picFromDisc == null)
            {
                CameraCaptureUI captureUI = new CameraCaptureUI();
                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
                captureUI.PhotoSettings.AllowCropping = false;
                photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            }
            else
            {
                photo = picFromDisc;
            }



            if (photo == null)
            {
                // User cancelled photo capture
                return;
            }
            StorageFolder destinationFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("ProfilePhotoFolder",
                  CreationCollisionOption.OpenIfExists);

            await photo.CopyAsync(destinationFolder, "ProfilePhoto.jpg", NameCollisionOption.ReplaceExisting);

            IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();


            SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

            this.Image = bitmapSource;



            var byteArray = await GetImageAsByteArray(photo);

            var response = await ImageAnalyze.GetImageInfo.GetInfo(byteArray: byteArray, key: App.Key);


            ApiAnswer.Clear();
            foreach (var item in response.Description.Captions)
            {
                var answer = new AiAnswer
                {
                    Description =
                    ($"Description: {item.Text} \nConfidence: {item.Confidence}")
                };
                ApiAnswer.Add(answer);

            }

        }

        public async Task<string> OpenKeyWindow()
        {
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputTextBox;
            dialog.Title = "You need an Api Key for Microsoft Cognitive Services to run this app.";
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return "";
        }

        public async Task<byte[]> GetImageAsByteArray(StorageFile photo)
        {
            // Open a read-only file stream for the specified file.
            using (var stream = await photo.OpenStreamForReadAsync())
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(stream);
                return binaryReader.ReadBytes((int)stream.Length);
            }
        }
    }
    public class AiAnswer : INotifyPropertyChanged
    {
        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
