using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Video.Resources;
using libvideo;
using TextureGraph;
using Windows.Phone.Media.Capture;

namespace Video
{
    public partial class MainPage : PhoneApplicationPage
    {
        TextureGraphInterop texGraph;
        Camera cam;

        public MainPage()
        {
            InitializeComponent();
        }

        // Just like last time with the LineGraph, we need to hook up our TextureGraph
        private void videoCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            texGraph = new TextureGraphInterop();

            // Set window bounds in dips
            texGraph.WindowBounds = new Windows.Foundation.Size(
                (float)videoCanvas.ActualWidth,
                (float)videoCanvas.ActualHeight
                );

            // Set native resolution in pixels
            texGraph.NativeResolution = new Windows.Foundation.Size(
                (float)Math.Floor(videoCanvas.ActualWidth * Application.Current.Host.Content.ScaleFactor / 100.0f + 0.5f),
                (float)Math.Floor(videoCanvas.ActualHeight * Application.Current.Host.Content.ScaleFactor / 100.0f + 0.5f)
                );

            // Set render resolution to the full native resolution
            texGraph.RenderResolution = texGraph.NativeResolution;

            // Hook-up native component to DrawingSurface
            videoCanvas.SetContentProvider(texGraph.CreateContentProvider());
            videoCanvas.SetManipulationHandler(texGraph);

            // Set the capture size of libvideo
            Windows.Foundation.Size captureSize = new Windows.Foundation.Size(1280, 720);

            // Construct libvideo's Camera object
            cam = new Camera(captureSize, CameraSensorLocation.Back);

            // When we have an input frame, call TextureGraphInterop::setTexturePtr
            cam.OnFrameReady += texGraph.setTexturePtr;
        }
    }
}