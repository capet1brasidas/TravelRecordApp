using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TravelRecordApp.Helpers;
using TravelRecordApp.Model;

namespace TravelRecordApp.ViewModel
{
	public class HistoryViewModel
	{
		public ObservableCollection<Post> Posts { get; set; }

		private Post selectedPost;
		public Post SelectedPost { get
			{
				return selectedPost;
			}
			set
			{
				selectedPost = value;
				if(selectedPost != null)
				{
					App.Current.MainPage.Navigation.PushAsync(new TravelDetailPage(selectedPost));
				}

			}
		}


		public HistoryViewModel()
		{
			Posts = new ObservableCollection<Post>();


		}

		public async Task GetPosts()
		{
			Posts.Clear();
			var posts = await Firestore.Read();
			foreach(var post in posts)
			{
				Posts.Add(post);
			}

		}
	}
}

