using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Geolocator;
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
            foreach(var post in posts)
			{
				try
				{
                    var pinCoordinates = new Position(post.Latitude, post.Longitude);

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

				}
				catch (Exception ex)
				{

				}
			
			}
        }

        private async void GetLocation()
        {
		  var status = await CheckAndRequestLocationPermission();
		  if(status == PermissionStatus.Granted)
			{
				var location = await Geolocation.GetLocationAsync();

				var locator = CrossGeolocator.Current;
				locator.PositionChanged += Locator_PositionChanged;
				await locator.StartListeningAsync(new TimeSpan(0, 1, 0), 100);

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
			Position center = new Position(latitude, longitude);
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

