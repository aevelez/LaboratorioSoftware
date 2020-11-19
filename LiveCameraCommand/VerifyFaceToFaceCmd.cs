using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LiveCameraCommand
{
    public class VerifyFaceToFaceCmd
    {
        private readonly HttpClient _httpClient;

        public VerifyFaceToFaceCmd()
        {
            _httpClient = new HttpClient();
        }
        public async Task<VerifyResult> VerifyFaceToFaceAsync(Guid? faceid, Guid? target)
        {
            try
            {
                var dataToSend = new Dictionary<string, object>();
                dataToSend.Add("faceid", faceid);
                dataToSend.Add("target", target);
                var json = JsonConvert.SerializeObject(dataToSend);
                var buffer = Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await _httpClient.PostAsync(ConfigurationManager.AppSettings["FaceAPIWrapper"] + "Verify", byteContent);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<VerifyResult>(data);
            }
            catch (Exception)
            {
                return new VerifyResult();
                //throw;
            }

        }
    }
}
