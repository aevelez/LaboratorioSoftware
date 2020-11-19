using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace LiveCameraCommand
{
    public class DetectWithStreamCmd
    {
        public async Task<IList<DetectedFace>> DetectWithStreamAsync(Stream photo, string recognitionModel, string detectionModel)
        {
            try
            {
                using (var _httpClient = new HttpClient())
                {
                    byte[] databytes;
                    using (var br = new BinaryReader(photo))
                        databytes = br.ReadBytes((int)photo.Length);
                    ByteArrayContent bytes = new ByteArrayContent(databytes);
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    multiContent.Add(bytes, "file", "Photo");
                    var response = await _httpClient.PostAsync(ConfigurationManager.AppSettings["FaceAPIWrapper"], multiContent);
                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IList<DetectedFace>>(data);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
