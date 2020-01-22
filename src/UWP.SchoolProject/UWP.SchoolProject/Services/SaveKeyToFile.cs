using System;
using System.Threading.Tasks;

namespace UWP.SchoolProject.Services
{
    public class SaveKeyToFile
    {
        public static Windows.Storage.StorageFile ApiKey;


        public static async Task<string> ReadFromFileAsync()
        {
            Windows.Storage.StorageFolder storageFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;
            ApiKey =
                await storageFolder.CreateFileAsync("file.oo",
                    Windows.Storage.CreationCollisionOption.OpenIfExists);

            string text = await Windows.Storage.FileIO.ReadTextAsync(ApiKey);

            if (!string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return string.Empty;
        }

        public static async Task<string> WriteToFileAsync(string apiKey)
        {
            await Windows.Storage.FileIO.WriteTextAsync(ApiKey, apiKey);
            return apiKey;
        }
    }
}
