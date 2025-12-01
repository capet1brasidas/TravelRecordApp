using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using TravelRecordApp.Model;
using Xamarin.Forms;

namespace TravelRecordApp
{	
	public partial class HistoryPage : ContentPage
	{	
		public HistoryPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
			using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
			{
                conn.CreateTable<Post>();
                List<Post> posts = conn.Table<Post>().ToList();
                postListView.ItemsSource = posts;
            }
           

        }

        void postListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var selectdPost = postListView.SelectedItem as Post;
            if(selectdPost != null)
            {
                Navigation.PushAsync(new PostDetailPage(selectdPost));
            }
        }
    }
}

