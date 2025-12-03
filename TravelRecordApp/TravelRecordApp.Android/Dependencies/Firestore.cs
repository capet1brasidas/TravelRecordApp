//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Android.Gms.Extensions;
//using Android.Gms.Tasks;
//using Firebase.Auth;
//using Firebase.Firestore;
//using Java.Interop;
//using Java.Util;
//using TravelRecordApp.Helpers;
//using TravelRecordApp.Model;
//using Xamarin.Forms;

//[assembly: Dependency(typeof(TravelRecordApp.Droid.Dependencies.Firestore))]
//namespace TravelRecordApp.Droid.Dependencies
//{
//	public class Firestore :Java.Lang.Object, IFirestore
//	{
//		public Firestore()
//		{
            
//		}

//        public async Task<bool> Delete(Post post)
//        {
//            try
//            {
//                var collection = Firebase.Firestore.FirebaseFirestore.Instance.Collection("posts");
//                collection.Document(post.Id).Delete();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public bool Insert(Post post)
//        {
//            try
//            {
                
//                Console.Write("aaaa");

//                var userId = FirebaseAuth.Instance.CurrentUser.Uid;

//                Console.Write("aaaa");
//                var postDocument = new Dictionary<string, Java.Lang.Object>
//            {
//                {"experience", post.Experience },
//                { "categoryId", post.CategoryId },
//                { "categoryName", post.CategoryName },
//                { "latitude", post.Latitude },
//                { "longitude", post.Longitude },
//                { "distance", post.Distance },
//                { "address", post.Address },
//                { "venueName", post.VenueName },
//                { "userId", Firebase.Auth.FirebaseAuth.Instance.CurrentUser.Uid },

//            };

//                var collection = FirebaseFirestore.Instance.Collection("posts");


//                Console.Write("aaaa");

//                collection.Add(new HashMap(postDocument));

//                return true;
//            }catch(Exception ex)
//            {
//                throw new Exception(ex.Message);
//                return false;
//            }
            
//        }


//        public async Task<List<Post>> Read()
//        {

//            //var db = FirebaseFirestore.Instance;
//            //if (db == null)
//            //{
//            //    System.Diagnostics.Debug.WriteLine("Read(): CurrentUser == null, 用户未登录");
//            //    return new List<Post>();
//            //}

//            var collection = FirebaseFirestore.Instance.Collection("posts");

//            //Console.Write("aaa");

//            var result = await FirebaseFirestore.Instance
//                .Collection("posts")
//                .WhereEqualTo("userId", FirebaseAuth.Instance.CurrentUser.Uid)
//                .Get()
//                .AsAsync<QuerySnapshot>(); // 转 Task<QuerySnapshot>

//            List<Post> posts = new List<Post>();

//            foreach (var doc in result.Documents)
//            {
//                posts.Add(new Post
//                {
//                    Experience = doc.Get("experience").ToString(),
//                    CategoryId = doc.Get("categoryId").ToString(),
//                    CategoryName = doc.Get("categoryName").ToString(),
//                    Latitude = (double)doc.Get("latitude"),
//                    Longitude = (double)doc.Get("longitude"),
//                    Distance = (int)doc.Get("distance"),
//                    Address = doc.Get("address").ToString(),
//                    VenueName = doc.Get("venueName").ToString(),
//                    UserId = doc.Get("userId").ToString(),
//                    Id = doc.Id
//                });
//            }

//            return posts;
//        }


//        public async Task<bool> Update(Post post)
//        {
//            try
//            {
//                var postDocument = new Dictionary<string, Java.Lang.Object>
//            {
//                {"experience", post.Experience },
//                { "categoryId", post.CategoryId },
//                { "categoryName", post.CategoryName },
//                { "latitude", post.Latitude },
//                { "longitude", post.Longitude },
//                { "distance", post.Distance },
//                { "address", post.Address },
//                { "venueName", post.VenueName },
//                { "userId", Firebase.Auth.FirebaseAuth.Instance.CurrentUser.Uid },

//            };

//                var collection = Firebase.Firestore.FirebaseFirestore.Instance.Collection("posts");
//                collection.Document(post.Id).Update(postDocument);

//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }
//    }
//}

