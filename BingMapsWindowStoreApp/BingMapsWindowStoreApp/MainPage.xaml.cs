using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Bing.Maps;
using Windows.Devices.Geolocation;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BingMapsWindowStoreApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Class for accessing geographic location
        Geolocator glocator;
        //User control object
        PositionLocator pos;
        //class for containing coordinates of location on map
        Location location;


        public MainPage()
        {
            this.InitializeComponent();
            glocator = new Geolocator();
            pos = new PositionLocator();
            myMap.Children.Add(pos);
            //S3: Event of the Position Changed
            glocator.PositionChanged += new Windows.Foundation.TypedEventHandler<Geolocator, PositionChangedEventArgs>(glocator_PositionChanged);

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnlocateme_Click_1(object sender, RoutedEventArgs e)
        {
            location = new Location(Convert.ToDouble(txtlatitude.Text), Convert.ToDouble(txtlongitude.Text));
            myMap.SetView(location, 12.0f);
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            //S1: Set the Default display for the MAp Type
            myMap.MapType = MapType.Aerial;
            //S2: Set the properties for the Map
            myMap.ShowBuildings = true;
            myMap.ShowTraffic = true;
            // myMap.ViewRestriction = MapViewRestriction.ZoomOutToWholeWorld;
        }

        //Async Execution of the event
        async void glocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(
              () =>
              {
                  GetMyPosition(this, args);
              }));
        }

        /// <summary>
        /// Method to get the default position of the application user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void GetMyPosition(object sender, PositionChangedEventArgs args)
        {
            location = new Location(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);
            //S4: Set the Position on the Map using location values and the put the UI for postion (Rectangle)
            MapLayer.SetPosition(pos, location);
            //S5: Set the Map view for the location values and zoom level
            myMap.SetView(location, 18.0f);

            //Display the Longitude and Latitude values
            txtlatitude.Text = args.Position.Coordinate.Latitude.ToString();
            txtlongitude.Text = args.Position.Coordinate.Longitude.ToString();
        }
    }
}
