using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;                 // 确保已引用 Newtonsoft.Json
using TravelRecordApp.Model;
using Xamarin.Forms;

namespace TravelRecordApp.Helpers
{
    // 如果别的地方已经不用 IFirestore，可以把这个 interface 删掉；
    // 如果还在用，可以先留着但不要再用 DependencyService.Get<IFirestore>() 了。
    public interface IFirestore
    {
        Task<bool> Insert(Post post);
        Task<bool> Delete(Post post);
        Task<bool> Update(Post post);
        Task<List<Post>> Read();
    }

    // 用 HttpClient 直接调 Cloud Functions
    public class Firestore
    {
        // 你的 Cloud Functions base url
        // 末尾一定要有 /，方便拼路径
        private const string BaseUrl =
            "https://us-central1-travelrecordapp-16f36.cloudfunctions.net/";

        private static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };

        public Firestore()
        {
        }

      

        // ============== 实际实现部分（调 Cloud Functions） ==============

        /// <summary>
        /// 调用 createPost Cloud Function
        /// </summary>
        public static async Task<bool> Insert(Post post)
        {
            try
            {

                var auth = DependencyService.Get<IAuth>();
                var userId = auth?.GetCurrentUserId();
                var payload = new
                {
                    experience = post.Experience,
                    categoryId = post.CategoryId,
                    categoryName = post.CategoryName,
                    latitude = post.Latitude,
                    longitude = post.Longitude,
                    distance = post.Distance,
                    address = post.Address,
                    venueName = post.VenueName,
                    userId = userId     // 记得提前在 Post 里填好
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("createPost", content);

                if (!response.IsSuccessStatusCode)
                    return false;

                // 可选：解析 success 字段
                var body = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<BasicResponse>(body);
                return obj?.success == true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 调用 updatePost Cloud Function
        /// </summary>
        public static async Task<bool> Update(Post post)
        {
            if (string.IsNullOrEmpty(post.Id))
                return false;


            try
            {
                var payload = new
                {
                    id = post.Id,
                    experience = post.Experience,
                    categoryId = post.CategoryId,
                    categoryName = post.CategoryName,
                    latitude = post.Latitude,
                    longitude = post.Longitude,
                    distance = post.Distance,
                    address = post.Address,
                    venueName = post.VenueName,
                    userId = post.UserId
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // updatePost 支持 POST / PUT / PATCH，这里用 POST
                var response = await httpClient.PostAsync("updatePost", content);

                if (!response.IsSuccessStatusCode)
                    return false;

                var body = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<BasicResponse>(body);
                return obj?.success == true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 调用 deletePost Cloud Function
        /// </summary>
        public static async Task<bool> Delete(Post post)
        {
            if (string.IsNullOrEmpty(post.Id))
                return false;

            try
            {
                var payload = new { id = post.Id };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("deletePost", content);

                if (!response.IsSuccessStatusCode)
                    return false;

                var body = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<BasicResponse>(body);
                return obj?.success == true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 调用 getPosts Cloud Function，按当前用户 userId 获取
        /// </summary>
        public static async Task<List<Post>> Read()
        {
            try
            {
                // 从你的 Auth helper 里取当前用户 Id
                // 这里假设有 IAuth 接口 + DependencyService
                var auth = DependencyService.Get<IAuth>();
                var userId = auth?.GetCurrentUserId();

                if (string.IsNullOrEmpty(userId))
                    return new List<Post>();

                var url = $"getPosts?userId={Uri.EscapeDataString(userId)}";

                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return new List<Post>();

                var body = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<GetPostsResponse>(body);

                if (data?.success != true || data.posts == null)
                    return new List<Post>();

                // 映射成你的 Post 模型
                return data.posts
                    .Select(p => new Post
                    {
                        Id = p.id,
                        Experience = p.experience,
                        CategoryId = p.categoryId,
                        CategoryName = p.categoryName,
                        Latitude = p.latitude,
                        Longitude = p.longitude,
                        Distance = p.distance,
                        Address = p.address,
                        VenueName = p.venueName,
                        UserId = p.userId
                    })
                    .ToList();
            }
            catch
            {
                return new List<Post>();
            }
        }

        // ==================== 辅助 DTO ====================

        private class BasicResponse
        {
            public bool success { get; set; }
        }

        private class GetPostsResponse
        {
            public bool success { get; set; }
            public List<PostDto> posts { get; set; }
        }

        private class PostDto
        {
            public string id { get; set; }
            public string experience { get; set; }
            public string categoryId { get; set; }
            public string categoryName { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public int distance { get; set; }
            public string address { get; set; }
            public string venueName { get; set; }
            public string userId { get; set; }
        }
    }
}