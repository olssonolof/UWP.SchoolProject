
using ImageAnalyze.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImageAnalyze
{
    public class GetImageInfo
    {


        public static async Task<ImageInfo> GetInfo(string path = null, byte[] byteArray = null, string key = null)
        {
            if (key == null)
            {
                throw new Exception("No ApiKey provided.");
            }


            byte[] byteData;
            var contentString = new ImageInfo();
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://northeurope.api.cognitive.microsoft.com");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);


            HttpResponseMessage response = new HttpResponseMessage();
            if (!string.IsNullOrWhiteSpace(path))
            {
                byteData = GetImageAsByteArray(path);
            }
            else
            {
                byteData = byteArray;
            }
            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                response = await client.PostAsync("/vision/v1.0/analyze?visualFeatures=description&details=landmarks", content);
                string stringResponse = await response.Content.ReadAsStringAsync();
                contentString = JsonConvert.DeserializeObject<ImageInfo>(await response.Content.ReadAsStringAsync());

            }

            return contentString;

        }


        static byte[] GetImageAsByteArray(string path)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

    }
}
