using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using LineGraphLifecycle.Resources;
using LineGraph;
using System.Windows.Threading;

namespace LineGraphLifecycle
{
    public partial class MainPage : PhoneApplicationPage
    {
        // This is the graph that does things in the proper way
        LineGraphInterop topGraph;

        // This is the graph that does NOT do things in the proper way
        LineGraphInterop botGraph;

        // This is what we'll use to push stuff out onto the lineGraphs with
        DispatcherTimer dt;
        float[] data = new float[256];

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            logString("MainPage()");

            // Initialize DispatcherTimer to run 50 times a second
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(20);
            dt.Tick += dt_Tick;
            dt.Start();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            // Output slowly shifting Sinusoids
            double phaseOffset = DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0;
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = (float)Math.Sin(i*2*Math.PI/data.Length + phaseOffset);
            }

            // Only do this if they're not null, since we don't know exactly when the graphs will be ready
            if( topGraph != null )
                topGraph.setArray(data);
            if (botGraph != null)
                botGraph.setArray(data);
        }

        // Generic way to hook a LineGraphInterop up with a DrawingSurface
        private LineGraphInterop initializeLineGraph(DrawingSurface canvas)
        {
            // Create the LineGraphInterop, the C++/CX component that will draw out to the canvas
            LineGraphInterop graph = new LineGraphInterop();

            // Set window bounds
            graph.WindowBounds = new Windows.Foundation.Size(
                (float)canvas.ActualWidth,
                (float)canvas.ActualHeight
                );

            // Set native/rendering resolution!
            graph.NativeResolution = new Windows.Foundation.Size(
                (float)Math.Floor(canvas.ActualWidth * Application.Current.Host.Content.ScaleFactor / 100.0f + 0.5f),
                (float)Math.Floor(canvas.ActualHeight * Application.Current.Host.Content.ScaleFactor / 100.0f + 0.5f)
                );
            graph.RenderResolution = graph.NativeResolution;

            // Notify the canvas who we're hooking up to them
            canvas.SetContentProvider(graph.CreateContentProvider());
            canvas.SetManipulationHandler(graph);

            // Give back the graph object!
            return graph;
        }

        private void topCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            logString("topCanvas_Loaded()");

            // Initialize the lineGraph (this only happens ONCE per app lifecycle!)
            topGraph = initializeLineGraph(topCanvas);
            topGraph.Initialized += setupTopCanvas;

            // Note that we do have to call it initially, as the Initialized event has already fired by this point.  :P
            setupTopCanvas();
        }

        private void botCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            logString("botCanvas_Loaded()");

            // Initialize the lineGraph (this only happens ONCE per app lifecycle!)
            botGraph = initializeLineGraph(botCanvas);

            // Set configuration settings of botCanvas.  This configuration will get lost!
            botGraph.setColor(0.0f, 0.0f, 1.0f);
            botGraph.setYLimits(-1.5f, 1.5f);
        }

        private void setupTopCanvas()
        {
            logString("setupTopCanvas()");

            // Configure the settings here, which gets called for topCanvas_Loaded(), as well as when we're resuming!
            topGraph.setColor(1.0f, 0.0f, 0.0f);
            topGraph.setYLimits(-1.5f, 1.5f);
        }


        // This takes in a string message, prepends the current time, then appends the whole line to the logOut TextBlock
        private void logString(string message)
        {
            this.logOut.Text += "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]: " + message + "\n";
        }

        

    }
}