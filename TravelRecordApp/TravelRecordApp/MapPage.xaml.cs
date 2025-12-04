using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using SQLite;
using TravelRecordApp.Helpers;
using TravelRecordApp.Model;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TravelRecordApp
{
	
    public partial class MapPage : ContentPage
	{
        private IGeolocator geolocator = CrossGeolocator.Current;

        public MapPage ()
		{
			InitializeComponent ();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			GetLocation();

			GetPosts();
		}

        private async void GetPosts()
        {
			//        using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
			//        {
			//            conn.CreateTable<Post>();
			//            List<Post> posts = conn.Table<Post>().ToList();
			//DisplayOnMap(posts);
			//        }

			var posts = await Firestore.Read();
			DisplayOnMap(posts);
        }

        private void DisplayOnMap(List<Post> posts)
        {
            foreach(Post post in posts)
			{
				try
				{
					if(post.Latitude == null || post.Longitude == null)
					{
						continue;
					}
                    var pinCoordinates = new Xamarin.Forms.Maps.Position(post.Latitude, post.Longitude);

                    var pin = new Pin()
                    {
                        Position = pinCoordinates,
                        Label = post.VenueName,
                        Address = post.Address,
                        Type = PinType.SavedPin,

                    };

                    locationsMap.Pins.Add(pin);
				}
				catch (NullReferenceException nre)
				{
					throw new Exception(nre.Message);
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
			
			}
        }

        private async void GetLocation()
        {
		  var status = await CheckAndRequestLocationPermission();
		  if(status == PermissionStatus.Granted)
			{
				var location = await Geolocation.GetLocationAsync();


                geolocator.PositionChanged += Locator_PositionChanged;
				if (!geolocator.IsListening)
				{
                    await geolocator.StartListeningAsync(new TimeSpan(0, 1, 0), 100);
                }
				locationsMap.IsShowingUser = true;

				CenterMap(location.Latitude,location.Longitude);

			}
        }

        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
			CenterMap(e.Position.Latitude, e.Position.Longitude);
        }

        private void CenterMap(double latitude, double longitude)
        {
            Xamarin.Forms.Maps.Position center = new Xamarin.Forms.Maps.Position(latitude, longitude);
			MapSpan span = new MapSpan(center,1,1);

			locationsMap.MoveToRegion(span);
        }

        private async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
			var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

			if(status == PermissionStatus.Granted)
			{
				return status;
			}

			if(status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
			{
				//prompt the user to turn on the permission in settings
				Console.Write("Please turn on your allow location");

			}

			status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

			return status;
        }
    }
}

