using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TravelRecordApp.Helpers;
using TravelRecordApp.Model;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModel
{
	public class NewTravelViewModel : INotifyPropertyChanged
	{
		public ObservableCollection<Venue> Venues { get; set; }

		public Command SaveCommand { get; set; }

		private Venue selectedVenue;
		public Venue SelectedVenue
		{
			get
			{
				return selectedVenue;
			}
			set
			{
				selectedVenue = value;
				OnpropertyChanged("PostIsReady");
			}
		}

		private bool postIsReady;
		public bool PostIsReady
		{
			get { return !string.IsNullOrEmpty(Experience) && SelectedVenue!=null; }
		}

		private string experience;
		public string Experience {
			get
			{
				return experience; 
			}
			set
			{
				experience = value;
				OnpropertyChanged("Experience");
                OnpropertyChanged("PostIsReady");
            }
		}

		public NewTravelViewModel()
		{
			Venues = new ObservableCollection<Venue>();

            SaveCommand = new Command<bool>(
			   async (parameter) => await Save(parameter),
			   CanSave
		   );
        }

		private async Task Save(bool parameter)
		{
			try { 
          
            var firstCategory = SelectedVenue.categories.FirstOrDefault();
            Post post = new Post()
            {
                Experience = Experience,
                CategoryId = firstCategory.fsq_category_id,
                CategoryName = firstCategory.name,
                Address = selectedVenue.location.address,
                Distance = selectedVenue.distance,
                Latitude = selectedVenue.latitude,
                Longitude = selectedVenue.longitude,
                VenueName = selectedVenue.name
            };

            bool result = await Firestore.Insert(post);
            if (result)
            {
                App.Current.MainPage.DisplayAlert("Success", "Experience successfully inserted", "OK");
                Experience = string.Empty;

            }
            else
            {
                    App.Current.MainPage.DisplayAlert("Failure", "Experience didn't insert", "OK");
            }

			}
            catch (NullReferenceException nre)
            {
                App.Current.MainPage.DisplayAlert("Failure", nre.ToString(), "OK");
			}
			catch(Exception ex)
			{
                App.Current.MainPage.DisplayAlert("Failure", ex.Message, "OK");
    //throw new Exception(ex.Message);
			}

        }

		private bool CanSave(bool parameter)
		{

            return parameter;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task GetVenues(double lat, double lng)
		{
            var venues = await VenueRoot.GetVenues(lat, lng);

			Venues.Clear();

			foreach(var venue in venues)
			{
				Venues.Add(venue);
			}
        }

		private void OnpropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

