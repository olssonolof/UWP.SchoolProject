using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UWP.SchoolProject.ViewModels
{
    class CameraViewModel : ViewModelBase
    {
        private ImageSource image;

        public bool imageExist;

        public bool ImageExist
        {
            get => imageExist;
            set
            {
                imageExist = ImageFile == null ? false : true;
                NotifyPropertyChanged();
            }
        }

        private bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                NotifyPropertyChanged();
            }
        }


        public ImageSource Image
        {
            get => image;
            set
            {
                image = value;
                NotifyPropertyChanged();
            }
        }

        private StorageFile imageFile;
        public StorageFile ImageFile
        {
            get => imageFile;
            set
            {
                imageFile = value;
                ImageExist = true;
            }
        }


        public ObservableCollection<AiAnswer> ApiAnswer { get; set; }

        public SoftwareBitmap FileToDisk { get; set; }


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
            ImageFile = null;

            if (picFromDisc == null)
            {
                CameraCaptureUI captureUI = new CameraCaptureUI();
                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
                captureUI.PhotoSettings.AllowCropping = false;
                ImageFile = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            }
            else
            {
                ImageFile = picFromDisc;
            }



            if (ImageFile == null)
            {
                // User cancelled photo capture
                return;
            }
            StorageFolder destinationFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("ProfilePhotoFolder",
                  CreationCollisionOption.OpenIfExists);

            await ImageFile.CopyAsync(destinationFolder, "ProfilePhoto.jpg", NameCollisionOption.ReplaceExisting);

            IRandomAccessStream stream = await ImageFile.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();

            FileToDisk = softwareBitmap;

            SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

            this.Image = bitmapSource;

            ApiAnswer.Clear();
            this.IsLoading = true;

            var byteArray = await GetImageAsByteArray(ImageFile);

            var response = await ImageAnalyze.GetImageInfo.GetInfo(byteArray: byteArray, key: App.Key);


            this.IsLoading = false;
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

        public async Task SaveImageToDisc()
        {
            FileSavePicker fileSavePicker = new FileSavePicker();
            fileSavePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileSavePicker.FileTypeChoices.Add("JPEG files", new List<string>() { ".jpg" });
            fileSavePicker.SuggestedFileName = "image";

            var outputFile = await fileSavePicker.PickSaveFileAsync();

            if (outputFile == null)
            {
                // The user cancelled the picking operation
                return;
            }
            await SaveSoftwareBitmapToFile(FileToDisk, outputFile);
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
        public async Task SaveSoftwareBitmapToFile(SoftwareBitmap softwareBitmap, StorageFile outputFile)
        {
            using (IRandomAccessStream stream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                // Create an encoder with the desired format
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);

                // Set the software bitmap
                encoder.SetSoftwareBitmap(softwareBitmap);

                // Set additional encoding parameters, if needed
                encoder.BitmapTransform.ScaledWidth = (uint)softwareBitmap.PixelWidth;
                encoder.BitmapTransform.ScaledHeight = (uint)softwareBitmap.PixelHeight;
                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
                encoder.IsThumbnailGenerated = true;

                try
                {
                    await encoder.FlushAsync();
                }
                catch (Exception err)
                {
                    const int WINCODEC_ERR_UNSUPPORTEDOPERATION = unchecked((int)0x88982F81);
                    switch (err.HResult)
                    {
                        case WINCODEC_ERR_UNSUPPORTEDOPERATION:
                            // If the encoder does not support writing a thumbnail, then try again
                            // but disable thumbnail generation.
                            encoder.IsThumbnailGenerated = false;
                            break;
                        default:
                            throw;
                    }
                }

                if (encoder.IsThumbnailGenerated == false)
                {
                    await encoder.FlushAsync();
                }


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
