using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AppLifecycle.Resources;

namespace AppLifecycle
{
    public partial class MainPage : PhoneApplicationPage
    {
        // This takes in a string message, prepends the current time, then appends the whole line to the logOut TextBlock
        private void logString( string message )
        {
            this.logOut.Text += "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]: " + message + "\n";
        }

        public MainPage()
        {
            InitializeComponent();

            logString("MainPage()");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // This tells us what NavigationType we're dealing with.  With this, we can disambiguate
            // whether we're opening this page for the first time, or coming back to it from an interruption, etc...
            string navigationType = "";
            switch (e.NavigationMode)
            {
                case NavigationMode.Back:
                    navigationType = "BACK";
                    break;
                case NavigationMode.Forward:
                    navigationType = "FORWARD";
                    break;
                case NavigationMode.New:
                    navigationType = "NEW";
                    break;
                case NavigationMode.Refresh:
                    navigationType = "REFRESH";
                    break;
                case NavigationMode.Reset:
                    navigationType = "RESET";
                    break;
            }
            logString("OnNavigatedTo(): " + navigationType);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            logString("OnNavigatedFrom()");
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            logString("OnNavigatingFrom()");
            base.OnNavigatingFrom(e);
        }
    }
}