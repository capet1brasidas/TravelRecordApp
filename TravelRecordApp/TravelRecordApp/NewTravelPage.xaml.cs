using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.Geolocator;
using SQLite;
using TravelRecordApp.Logic;
using TravelRecordApp.Model;
using Xamarin.Forms;

namespace TravelRecordApp
{	
	public partial class NewTravelPage : ContentPage
	{	
		public NewTravelPage ()
		{
			InitializeComponent ();
		}

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
			try
			{
                var selectedVenue = venueListView.SelectedItem as Venue;
                var firstCategory = selectedVenue.categories.FirstOrDefault();
                Post post = new Post()
                {
                    Experience = experienceEntry.Text,
                    CategoryId = firstCategory.fsq_category_id,
                    CategoryName = firstCategory.name,
                    Address = selectedVenue.location.address,
                    Distance = selectedVenue.distance,
                    Latitude = selectedVenue.latitude,
                    Longitude = selectedVenue.longitude,
                    VenueName = selectedVenue.name
                };

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Post>();
                    int rows = conn.Insert(post);

                    if (rows > 0)
                    {
                        DisplayAlert("Success", "Experience successfully inserted", "OK");
                        experienceEntry.Text = "";

                    }
                    else
                    {
                        DisplayAlert("Failure", "Experience didn't insert", "OK");
                    }
                }
            }
            catch(NullReferenceException nre)
            {
               
            }
			catch(Exception ex)
			{

			}
			
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

			var locator = CrossGeolocator.Current;
			var position = await locator.GetPositionAsync();

			var venues = await VenueLogic.GetVenues(position.Latitude,position.Longitude);

			venueListView.ItemsSource = venues;

        }
    }
}

