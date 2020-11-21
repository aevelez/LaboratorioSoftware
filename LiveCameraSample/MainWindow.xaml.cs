// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: http://www.microsoft.com/cognitive
// 
// Microsoft Cognitive Services Github:
// https://github.com/Microsoft/Cognitive
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using LiveCameraCommand;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using VideoFrameAnalyzer;
using FaceAPI = Microsoft.Azure.CognitiveServices.Vision.Face;

namespace LiveCameraSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private readonly FrameGrabber<LiveCameraResult> _grabber;
        private static readonly ImageEncodingParam[] s_jpegParams = {
            new ImageEncodingParam(ImwriteFlags.JpegQuality, 60)
        };
        private LiveCameraResult _latestResultsToDisplay = null;

        private DateTime _startTime;

        public enum AppMode
        {
            Faces
        }

        public MainWindow()
        {
            InitializeComponent();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Create grabber. 
            _grabber = new FrameGrabber<LiveCameraResult>();

            // Set up a listener for when the client receives a new frame.
            _grabber.NewFrameProvided += (s, e) =>
            {
                // The callback may occur on a different thread, so we must use the
                // MainWindow.Dispatcher when manipulating the UI. 
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    // Display the image in the left pane.
                    LeftImage.Source = e.Frame.Image.ToBitmapSource();
                }));

                // See if auto-stop should be triggered. 
                if (Properties.Settings.Default.AutoStopEnabled && (DateTime.Now - _startTime) > Properties.Settings.Default.AutoStopTime)
                {
                    _grabber.StopProcessingAsync().GetAwaiter().GetResult();
                }
            };

            // Set up a listener for when the client receives a new result from an API call. 
            _grabber.NewResultAvailable += (s, e) =>
            {
                this.Dispatcher.BeginInvoke((Action)(async () =>
                {
                    if (e.TimedOut)
                    {
                        MessageArea.Text = "API call timed out.";
                    }
                    else if (e.Exception != null)
                    {
                        string apiName = "";
                        string message = e.Exception.Message;
                        var faceEx = e.Exception as FaceAPI.Models.APIErrorException;
                        if (faceEx != null)
                        {
                            apiName = "Face";
                            message = faceEx.Message;
                        }

                        MessageArea.Text = string.Format("{0} API call failed on frame {1}. Exception: {2}", apiName, e.Frame.Metadata.Index, message);
                    }
                    else
                    {
                        _latestResultsToDisplay = e.Analysis;
                        if (_latestResultsToDisplay != null && _latestResultsToDisplay.Faces.Any())
                        {
                            if (!string.IsNullOrWhiteSpace(DocumentImagePath))
                            {
                                MessageArea.Text = "Verificando Rostro...";
                                if (await FindSimilar(_latestResultsToDisplay))
                                {
                                    await _grabber.StopProcessingAsync();
                                }
                            }
                            else
                            {
                                MessageArea.Text = "Por favor seleccione una imagen";
                            }
                        }

                    }
                }));
            };
        }

        public async Task<bool> FindSimilar(LiveCameraResult liveCameraResult)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(DocumentImagePath))
                    return false;
                IList<Guid?> targetFaceIds = new List<Guid?>();
                using (var jpg = File.OpenRead(DocumentImagePath))
                {

                    // Detect faces from load image.
                    var detectWithStreamCmd = new DetectWithStreamCmd();
                    var faces = await detectWithStreamCmd.DetectWithStreamAsync(jpg);

                    //// Add detected faceId to list of GUIDs.
                    if (faces.Count <= 0)
                    {
                        MessageArea.Text = $"No se detectaron rostros en la imagen.";
                        return false;
                    }
                    targetFaceIds.Add(faces[0].FaceId.Value);
                }
                var verifyFaceToFaceCmd = new VerifyFaceToFaceCmd();
                var similarResults = await verifyFaceToFaceCmd.VerifyFaceToFaceAsync(liveCameraResult.Faces.First().FaceId.Value, targetFaceIds.First().Value);

                if (similarResults.IsIdentical)
                {
                    RightImage.Source = VisualizeResult(liveCameraResult.VideoFrame);
                    MessageArea.Text = $"Los rostros son similares con una confianza de: {similarResults.Confidence}.";
                    return true;
                }
                else
                {
                    MessageArea.Text = $"Los rostros no son identicos.";
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageArea.Text = $"Se ha presentado un error: {ex.Message}";
                return false;
            }
        }

        /// <summary> Function which submits a frame to the Face API. </summary>
        /// <param name="frame"> The video frame to submit. </param>
        /// <returns> A <see cref="Task{LiveCameraResult}"/> representing the asynchronous API call,
        ///     and containing the faces returned by the API. </returns>
        private async Task<LiveCameraResult> FacesAnalysisFunction(VideoFrame frame)
        {
            // Encode image. 
            var jpg = frame.Image.ToMemoryStream(".jpg", s_jpegParams);
            // Submit image to API. 

            var detectWithStreamCmd = new DetectWithStreamCmd();
            var faces = await detectWithStreamCmd.DetectWithStreamAsync(jpg);
            // Count the API call. 
            Properties.Settings.Default.FaceAPICallCount++;
            // Output. 
            return new LiveCameraResult { Faces = faces.ToArray(), VideoFrame = frame };
        }

        private BitmapSource VisualizeResult(VideoFrame frame)
        {
            // Draw any results on top of the image. 
            BitmapSource visImage = frame.Image.ToBitmapSource();
            return visImage;
        }

        /// <summary> Populate CameraList in the UI, once it is loaded. </summary>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Routed event information. </param>
        private void CameraList_Loaded(object sender, RoutedEventArgs e)
        {
            int numCameras = _grabber.GetNumCameras();

            if (numCameras == 0)
            {
                MessageArea.Text = "No se encontraron cámaras!";
            }

            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = Enumerable.Range(0, numCameras).Select(i => string.Format("Cámara {0}", i + 1));
            comboBox.SelectedIndex = 0;
        }


        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraList.HasItems)
            {
                MessageArea.Text = "No cameras found; cannot start processing";
                return;
            }
            RightImage.Source = null;
            _grabber.AnalysisFunction = FacesAnalysisFunction;
            // How often to analyze. 
            _grabber.TriggerAnalysisOnInterval(Properties.Settings.Default.AnalysisInterval);

            // Reset message. 
            MessageArea.Text = "";

            // Record start time, for auto-stop
            _startTime = DateTime.Now;

            await _grabber.StartProcessingCameraAsync(CameraList.SelectedIndex);
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            await _grabber.StopProcessingAsync();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPanel.Visibility = 1 - SettingsPanel.Visibility;
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPanel.Visibility = Visibility.Hidden;
            Properties.Settings.Default.Save();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private string DocumentImagePath;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 

            dlg.Filter = "Image Files(*.JPG; *.JPEG; *.PNG)| *.JPG; *.PNG; *.JPEG";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                DocumentImagePath = dlg.FileName;
                // Open document 
                DocumentImage.Source = new BitmapImage(new Uri(dlg.FileName));

            }
        }
    }
}