using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LiveCamerServicesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LiveCameraController : ControllerBase
    {
        // GET <LiveCameraController>
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }



        // POST: <LiveCameraController>
        [HttpPost]
        public async Task<IList<DetectedFace>> Post()
        {
            try
            {

                IList<DetectedFace> faces = default;
                if (Request.HasFormContentType)
                {
                    if (Request.Form.Files.Any())
                    {
                        var _faceClient = new FaceClient(new ApiKeyServiceClientCredentials("540f5628a89945699eeccf5bd50aaca4"))
                        {
                            Endpoint = @"https://testlaboratioreconocimientofacial.cognitiveservices.azure.com"
                        };
                        // Detect faces from load image.
                        faces = await _faceClient.Face.DetectWithStreamAsync(Request.Form.Files.First().OpenReadStream(), recognitionModel: RecognitionModel.Recognition03, detectionModel: DetectionModel.Detection02);
                    }

                }
                return faces;
            }
            catch
            {
                return null;
            }

        }

        // POST: <LiveCameraController>
        [HttpPost]
        [Route("Verify")]
        public async Task<VerifyResult> PostVerify([FromBody] Dictionary<string, object> param)
        {
            try
            {
                var _faceClient = new FaceClient(new ApiKeyServiceClientCredentials("540f5628a89945699eeccf5bd50aaca4"))
                {
                    Endpoint = @"https://testlaboratioreconocimientofacial.cognitiveservices.azure.com"
                };

                var similarResults = await _faceClient.Face.VerifyFaceToFaceAsync(Guid.Parse(param["faceid"].ToString()), Guid.Parse(param["target"].ToString()));

                return similarResults;
            }
            catch
            {
                return null;
            }

        }
    }
}
