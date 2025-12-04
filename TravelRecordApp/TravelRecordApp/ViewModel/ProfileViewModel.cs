using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TravelRecordApp.Helpers;
using TravelRecordApp.Model;

namespace TravelRecordApp.ViewModel
{
	public class ProfileViewModel : INotifyPropertyChanged
	{
		public ObservableCollection<CategoryCOunt> Categories { get; set; }

        private int postCount;
        public int PostCount {
            get
            {
                return postCount;
            }
               
            set {
                postCount = value;
                OnPropertyChanged("PostCount");
            }
            }
        public event PropertyChangedEventHandler PropertyChanged;


        public ProfileViewModel()
		{
			Categories = new ObservableCollection<CategoryCOunt>();
            

		}

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public async Task GetPosts()
        {
            Categories.Clear();
            var posts = await Firestore.Read();

            PostCount = posts.Count();

            var categories = (from p in posts
                              orderby p.CategoryId
                              select p.CategoryName).Distinct().ToList();


            Dictionary<string, int> categoriesCount = new Dictionary<string, int>();
            foreach (var category in categories)
            {
                if (category == null)
                {
                    continue;
                }
                var count1 = (from post in posts
                              where post.CategoryName == category
                              select post).ToList().Count;

                var count2 = posts.Where(p => p.CategoryName == category).ToList().Count;

                Categories.Add(new CategoryCOunt()
                {
                    Name=category,
                    Count=count1
                });
            }

        }

        public class CategoryCOunt
        {
            public string Name { get; set; }
            public int Count { get; set; }


        }

    }
}

