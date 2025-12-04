using System;
using System.ComponentModel;
using TravelRecordApp.Helpers;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModel
{
	public class MainViewModel : INotifyPropertyChanged
	{
		public Command LoginCommand { get; set; }

        private string email;
        private string password;

        public string Email {
            get
            {
                return email;
            }


            set
            {
                email = value;
                onPropertyChanged("EntriesHaveText");
            } }
        public string Password {
            get {
                return password;
            }

            set {
                password = value;
                onPropertyChanged("EntriesHaveText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool entriesHaveText;
        public bool EntriesHaveText
        {
            get
            {
                return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
            }
        }

        public MainViewModel()
		{
            Email = "test@gmail.com";
            Password = "password123";
            LoginCommand = new Command<bool>(Login,CanLogin);
		}

        private void onPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

        private async void Login(bool parameter)
		{
           
                //authenticate
                bool result = await Auth.LoginUser(Email, Password);
                //navigate
                if (result)
                {
                    await App.Current.MainPage.Navigation.PushAsync(new HomePage());
                }
            }

        private bool CanLogin(bool parameter)
        {
            return EntriesHaveText;
        }
    }

    

    
}


