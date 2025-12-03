using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TravelRecordApp.Helpers
{
	public interface IAuth
	{
		Task<bool> RegisterUser(string email, string password);

        Task<bool> LoginUser(string email, string password);

        bool IsAuthenticated();

		string GetCurrentUserId();
    }


	public class Auth
	{
		private static IAuth auth = DependencyService.Get<IAuth>();

        public Auth()
		{
		}

		public async static Task<bool> RegisterUser(string email,string password)
		{

			return await auth.RegisterUser(email,password);
		}

        public async static Task<bool> LoginUser(string email, string password)
        {
			//if user does not exist, register
			try {
                return await auth.LoginUser(email, password);
            }catch(Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

				return await RegisterUser(email, password);
			}
          
        }

		public static bool IsAuthenticated()
		{
			return auth.IsAuthenticated();
		}

		public static string GetCurrentUserId()
		{

			return auth.GetCurrentUserId();
		}
    }
}

