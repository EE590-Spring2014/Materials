using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using LocksFixed.Resources;
using System.Threading;

namespace LocksFixed
{
    public partial class MainPage : PhoneApplicationPage
    {
        // This is a Timer, not a DispatcherTimer!  There is a BIG DIFFERENCE!
        private Timer t;
        private Object myLockObject = new Object();
        float[] data;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Initialize data as a relatively small array of zeros
            data = new float[1024];

            // Start a timer (that runs on a different thread!) that will run thread_tick() every 1 milliseconds
            t = new Timer(thread_tick, null, 0, 1);
        }

        public void thread_tick(Object ignored)
        {
            // Simply for demonstration's sake, let's save data.Length
            int oldLength = data.Length;

            // Calculate average of data:
            float avg = 0.0f;

            // lock(myLockObject) prevents any other thread from entering another lock(myLockObject) statement
            lock (myLockObject)
            {
                for (int i = 0; i < data.Length; ++i)
                {
                    avg += data[i];
                }
                avg /= data.Length;
            }

            // Output length to user
            Dispatcher.BeginInvoke(() =>
            {
                avgOut.Text = "Average: " + avg.ToString("0.00");
            });
        }

        private void randomizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a "Random" object so we can generate random variables
            Random r = new Random();

            // Calculate the new length of data
            int newLen = (int)(r.NextDouble() * 1000000);

            // lock(myLockObject) prevents any other thread from entering another lock(myLockObject) statement
            lock (myLockObject)
            {
                // Allocate space for data
                data = new float[newLen];

                // Fill data with randomness
                for (int i = 0; i < data.Length; ++i)
                {
                    data[i] = (float)r.NextDouble();
                }
            }
        }
    }
}