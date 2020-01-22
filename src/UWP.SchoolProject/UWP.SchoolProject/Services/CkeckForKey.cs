using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UWP.SchoolProject.Services
{
    public class CkeckForKey
    {

        public static async Task CheckIfKeyExist()
        {
            if (string.IsNullOrWhiteSpace(App.Key))
            {
                string key = await SaveKeyToFile.ReadFromFileAsync();

                if (string.IsNullOrWhiteSpace(key))
                {
                    key = await SaveKeyToFile.WriteToFileAsync(await OpenKeyWindow());
                }
                App.Key = key;
            }
        }


        public static async Task<string> OpenKeyWindow()
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

    }
}
