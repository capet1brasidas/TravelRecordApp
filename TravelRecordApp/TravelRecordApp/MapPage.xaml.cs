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

        private IGeolocator _locator;
        public MapPage ()
		{
			InitializeComponent ();
            _locator = CrossGeolocator.Current;
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();

			GetLocation();

			GetPosts();
		}

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            StopListening();   // 视情况选择是否真的在离开页面时停止
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
            try
            {
                var status = await CheckAndRequestLocationPermission();
                if (status != PermissionStatus.Granted)
                    return;

                // 先用 Essentials 拿一次当前位置居中
                var location = await Geolocation.GetLocationAsync();
                if (location != null)
                {
                    CenterMap(location.Latitude, location.Longitude);
                }

                // 避免重复挂事件：先移除再添加一次
                _locator.PositionChanged -= Locator_PositionChanged;
                _locator.PositionChanged += Locator_PositionChanged;

                // 如果已经在监听，就不要再 StartListening
                if (!_locator.IsListening)
                {
                    await _locator.StartListeningAsync(TimeSpan.FromMinutes(1), 100);
                }

                locationsMap.IsShowingUser = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetLocation error: {ex}");
            }
        }

        private async void StopListening()
        {
            try
            {
                if (_locator == null)
                    return;

                _locator.PositionChanged -= Locator_PositionChanged;

                if (_locator.IsListening)
                {
                    await _locator.StopListeningAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"StopListening error: {ex}");
            }
        }

        private void Locator_PositionChanged(object sender, PositionEventArgs e)
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

