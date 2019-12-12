using System.Threading.Tasks;
using Windows.Foundation;
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










        public async Task GetImageInfo()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.AllowCropping = false;

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

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

            var response = await ImageAnalyze.GetImageInfo.GetInfo(byteArray: byteArray);

            foreach (var item in response.Description.Captions)
            {
                this.Description += item.Text;

            }

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
}
