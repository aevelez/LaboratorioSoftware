using LiveCameraCommand;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PruebaApi
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.ReadLine();
                Task.Run(() => NewMethod());
                Console.ReadLine();
            }
            catch (Exception ex)
            {

                //throw;
            }
            
        }

        private static async void NewMethod()
        {
            //IList<Guid?> targetFaceIds = new List<Guid?>();
            //byte[] jpg = File.ReadAllBytes(@"C:\Users\Marco\Documents\images.jpg");
            //var detectWithStreamCmd = new DetectWithStreamCmd();
            //var faces = await detectWithStreamCmd.DetectWithStreamAsync(jpg, RecognitionModel.Recognition03, DetectionModel.Detection02);
            //if (faces.Count <= 0)
            //    Console.Write("no Faces detected in the image.");

            //targetFaceIds.Add(faces[0].FaceId.Value);

            ////287c7a8d-263c-4a25-ba5f-08c051c1494f

            //var verifyFaceToFaceCmd = new VerifyFaceToFaceCmd();
            //var similarResults = await verifyFaceToFaceCmd.VerifyFaceToFaceAsync(Guid.Parse("287c7a8d-263c-4a25-ba5f-08c051c1494f"), targetFaceIds.First().Value);

            //if (similarResults.IsIdentical)
            //{
            //    Console.Write($"Faces are similar with confidence: {similarResults.Confidence}");
            //    //return true;
            //}
            //else
            //{
            //    Console.Write($"Faces are not identical.");
            //    //return true;
            //}
        }
    }
}
