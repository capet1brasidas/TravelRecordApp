using System;
using System.Collections.Generic;
using SQLite;
using TravelRecordApp.Helpers;
using TravelRecordApp.Model;
using TravelRecordApp.ViewModel;
using Xamarin.Forms;

namespace TravelRecordApp
{	
	public partial class TravelDetailPage : ContentPage
	{

		public TravelDetailPage (Post selectedPost)
		{
			InitializeComponent ();
			var vm = Resources["vm"] as TravelDetailsViewModel;
            vm.SelectedPost = selectedPost;
			experienceEntry.Text = selectedPost.Experience;
        
		}

        
        }
}

