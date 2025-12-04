using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using TravelRecordApp.Model;
using Xamarin.Forms;
using TravelRecordApp.Helpers;
using TravelRecordApp.ViewModel;

namespace TravelRecordApp
{	
	public partial class HistoryPage : ContentPage
	{
        private HistoryViewModel vm;

		public HistoryPage ()
		{
			InitializeComponent ();

            vm = Resources["vm"] as HistoryViewModel;
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            //{
            //             conn.CreateTable<Post>();
            //             List<Post> posts = conn.Table<Post>().ToList();
            //             postListView.ItemsSource = posts;
            //         }
            //postListView.ItemsSource = null;

            //var posts = await Firestore.Read();
            //postListView.ItemsSource = posts;

            await vm.GetPosts();
           

        }

        void postListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var selectdPost = postListView.SelectedItem as Post;
            if(selectdPost != null)
            {
                Navigation.PushAsync(new TravelDetailPage(selectdPost));
            }
        }
    }
}

