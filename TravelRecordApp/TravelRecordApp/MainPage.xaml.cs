using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelRecordApp.Helpers;
using Xamarin.Forms;


namespace TravelRecordApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var assembly = typeof(MainPage);

            planeImage.Source = ImageSource.FromResource("plane.png",assembly);
            System.Diagnostics.Debug.WriteLine("RES: " + planeImage);

        }

        private async void loginButton_Clicked(System.Object sender, System.EventArgs e)
        {
            bool isEmailEmpty =  string.IsNullOrEmpty(emailEntry.Text);
            bool isPasswordEmpty = string.IsNullOrEmpty(passwordEntry.Text);

            if(isEmailEmpty || isEmailEmpty)
            {
                //do not navigate

            }
            else
            {
                //authenticate
               bool result = await Auth.LoginUser(emailEntry.Text, passwordEntry.Text);
                //navigate
                if (result)
                {
                    Navigation.PushAsync(new HomePage());
                }
            }

        }
    }
}

