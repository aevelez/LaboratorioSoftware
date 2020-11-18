using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LiveCamerServicesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LiveCameraController : ControllerBase
    {
        private FaceClient _faceClient = null;

        // GET <LiveCameraController>
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        // POST: <LiveCameraController>
        [HttpPost]
        public async Task<IList<DetectedFace>> Post([FromBody] Dictionary<string,object> param)
        {
            try
            {
                _faceClient = new FaceClient(new ApiKeyServiceClientCredentials("540f5628a89945699eeccf5bd50aaca4"))
                {
                    Endpoint = @"https://testlaboratioreconocimientofacial.cognitiveservices.azure.com"
                };
                var dencodedData = Convert.FromBase64String(param["photo"].ToString());
                System.IO.File.WriteAllBytes(@"C:\Users\Marco\Documents\hola.jpg", dencodedData);
                IList<DetectedFace> faces;
                using (var jpg = System.IO.File.OpenRead(@"C:\Users\Marco\Documents\hola.jpg"))
                {
                    // Detect faces from load image.
                    faces = await _faceClient.Face.DetectWithStreamAsync(jpg, recognitionModel: param["recognitionModel"].ToString(), detectionModel: param["detectionModel"].ToString());

                }
                return faces;
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }

        }

        // POST: <LiveCameraController>
        [HttpPost]
        [Route("Verify")]
        public async Task<VerifyResult> PostVerify([FromBody] Dictionary<string, object> param)
        {
            try
            {
                _faceClient = new FaceClient(new ApiKeyServiceClientCredentials("540f5628a89945699eeccf5bd50aaca4"))
                {
                    Endpoint = @"https://testlaboratioreconocimientofacial.cognitiveservices.azure.com"
                };

                var similarResults = await _faceClient.Face.VerifyFaceToFaceAsync(Guid.Parse(param["faceid"].ToString()), Guid.Parse(param["target"].ToString()));

                return similarResults;
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }

        }
    }
}
