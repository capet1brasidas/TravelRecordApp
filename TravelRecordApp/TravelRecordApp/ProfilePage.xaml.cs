using System;
using System.Collections.Generic;
using SQLite;
using TravelRecordApp.Model;
using Xamarin.Forms;
using System.Linq;
using TravelRecordApp.Helpers;

namespace TravelRecordApp
{	
	public partial class ProfilePage : ContentPage
	{	
		public ProfilePage ()
		{
			InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

			//using(SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
			//{
			//	var postTable = conn.Table<Post>().ToList();
			var postTable =await Firestore.Read();

			var categories = (from p in postTable
								  orderby p.CategoryId
								  select p.CategoryName).Distinct().ToList();

			var categories2 = postTable.OrderBy(p => p.CategoryId).Distinct().ToList();

			Dictionary<string, int> categoriesCount = new Dictionary<string, int>();
			foreach(var category in categories)
			{
					if(category == null)
					{
						continue;
					}
					var count1 = (from post in postTable
								 where post.CategoryName == category
								 select post).ToList().Count;

					var count2 = postTable.Where(p => p.CategoryName == category).ToList().Count;

					categoriesCount.Add(category, count1);
			}

				categoriesListView.ItemsSource = categoriesCount;



				postCountLabel.Text = postTable.Count.ToString();


			
        }
    }
}

