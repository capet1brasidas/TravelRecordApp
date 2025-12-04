//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using TravelRecordApp.Helpers;
//using TravelRecordApp.Model;
//using Xamarin.Forms;
//using Firebase.CloudFirestore;
//using Foundation;

//[assembly: Dependency(typeof(TravelRecordApp.iOs.Dependencies.Firestore))]
//namespace TravelRecordApp.iOs.Dependencies
//{
//    public class Firestore : IFirestore
//    {
//        public Firestore()
//        {
//        }

//        public async Task<bool> Delete(Post post)
//        {
//            try
//            {
//                var collection = Firebase.CloudFirestore.Firestore.SharedInstance.GetCollection("posts");
//                await collection.GetDocument(post.Id).DeleteDocumentAsync();
//                return true;
//            }catch(Exception ex)
//            {
//                return false;
//            }
            
//        }

//        public bool Insert(Post post)
//        {
//            try
//            {
//                var keys = new[]
//                {
//                    new NSString("experience"),
//                    new NSString("categoryId"),
//                    new NSString("categoryName"),
//                    new NSString("latitude"),
//                    new NSString("longitude"),
//                    new NSString("distance"),
//                    new NSString("address"),
//                    new NSString("venueName"),
//                    new NSString("userId"),
//                };

//                var values = new NSObject[]
//                {
//                    new NSString(post.Experience),
//                    new NSString(post.CategoryId),
//                    new NSString(post.CategoryName),
//                    new NSNumber(post.Latitude),
//                    new NSNumber(post.Longitude),
//                    new NSNumber(post.Distance),
//                    new NSString(post.Address),
//                    new NSString(post.VenueName),
//                    new NSString(Firebase.Auth.Auth.DefaultInstance.CurrentUser.Uid),
//                };

//                var document = new NSDictionary<NSString, NSObject>(keys, values);
//                var collection = Firebase.CloudFirestore.Firestore.SharedInstance.GetCollection("posts");
//                collection.AddDocument(document);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public async Task<List<Post>> Read()
//        {
//            var collection = Firebase.CloudFirestore.Firestore.SharedInstance.GetCollection("posts");
//            var query = collection.WhereEqualsTo("userId", Firebase.Auth.Auth.DefaultInstance.CurrentUser.Uid);

//            var documents = await query.GetDocumentsAsync();

//            List<Post> posts = new List<Post>();

//            foreach(var document in documents.Documents)
//            {
//                var dictionary = document.Data;

//                var newPost = new Post()
//                {
//                    Experience = dictionary.ValueForKey(new NSString("experience")) as NSString,
//                    VenueName = dictionary.ValueForKey(new NSString("venueName")) as NSString,
//                    CategoryId = dictionary.ValueForKey(new NSString("categoryId")) as NSString,
//                    CategoryName = dictionary.ValueForKey(new NSString("categoryName")) as NSString,
//                    Address = dictionary.ValueForKey(new NSString("address")) as NSString,
//                    Latitude = (double)(dictionary.ValueForKey(new NSString("latitude")) as NSNumber),
//                    Longitude = (double)(dictionary.ValueForKey(new NSString("longitude")) as NSNumber),
//                    Distance = (int)(dictionary.ValueForKey(new NSString("distance")) as NSNumber),
//                    UserId = dictionary.ValueForKey(new NSString("userId")) as NSString,
//                    Id = document.Id
//                };

//                posts.Add(newPost);
//            }

//            return posts;

//        }

//        public async Task<bool> Update(Post post)
//        {
//            try
//            {
//                var keys = new[]
//                {
//                    new NSString("experience"),
//                    new NSString("categoryId"),
//                    new NSString("categoryName"),
//                    new NSString("latitude"),
//                    new NSString("longitude"),
//                    new NSString("distance"),
//                    new NSString("address"),
//                    new NSString("venueName"),
//                    new NSString("userId"),
//                };

//                var values = new NSObject[]
//                {
//                    new NSString(post.Experience),
//                    new NSString(post.CategoryId),
//                    new NSString(post.CategoryName),
//                    new NSNumber(post.Latitude),
//                    new NSNumber(post.Longitude),
//                    new NSNumber(post.Distance),
//                    new NSString(post.Address),
//                    new NSString(post.VenueName),
//                    new NSString(Firebase.Auth.Auth.DefaultInstance.CurrentUser.Uid),
//                };

//                var document = new NSDictionary<NSObject, NSObject>(keys, values);
//                var collection = Firebase.CloudFirestore.Firestore.SharedInstance.GetCollection("posts");
//                await collection.GetDocument(post.Id).UpdateDataAsync(document);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }
//    }
//}

