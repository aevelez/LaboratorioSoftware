using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace LiveCameraCommand
{
    public class DetectWithStreamCmd
    {
        private readonly HttpClient _httpClient;

        public DetectWithStreamCmd()
        {
            _httpClient = new HttpClient();
        }
        public async Task<IList<DetectedFace>> DetectWithStreamAsync(byte[] photo, string recognitionModel, string detectionModel)
        {
            try
            {
                var dataToSend = new Dictionary<string, object>();
                dataToSend.Add("photo", photo);
                dataToSend.Add("recognitionModel", recognitionModel);
                dataToSend.Add("detectionModel", detectionModel);
                var json = JsonConvert.SerializeObject(dataToSend);
                var buffer = Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await _httpClient.PostAsync(@"https://localhost:44351/LiveCamera", byteContent);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IList<DetectedFace>>(data);
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }

        }
            
    }
}
