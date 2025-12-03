using System;
using System.Threading.Tasks;
using Foundation;
using TravelRecordApp.Helpers;
using Xamarin.Forms;

[assembly:Dependency(typeof(TravelRecordApp.iOS.Dependencies.Auth))]
namespace TravelRecordApp.iOS.Dependencies
{
	public class Auth:IAuth
	{
        

		public Auth()
		{

		}

        public string GetCurrentUserId()
        {
           return Firebase.Auth.Auth.DefaultInstance.CurrentUser.Uid;
        }

        public bool IsAuthenticated()
        {
            return Firebase.Auth.Auth.DefaultInstance.CurrentUser != null;
        }

        public async Task<bool> LoginUser(string email, string password)
        {
            try
            {
                await Firebase.Auth.Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
            }
            catch(NSErrorException error)
            {
                string message = error.Message.Substring(error.Message.IndexOf("NSLocalizedDescription=",StringComparison.CurrentCulture));
                message = message.Replace("NSLocalizedDescription=", "").Split(".")[0];


                throw new Exception(error.Message);
            }
            catch(Exception ex)
            {
                throw new Exception("There ws an unknown error " + ex.Message);
            }
          


           return true;
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            try
            {
                await Firebase.Auth.Auth.DefaultInstance.CreateUserAsync(email, password);
            }
            catch (NSErrorException error)
            {
                throw new Exception(error.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("There ws an unknown error " + ex.Message);
            }



            return true;
        }
    }
}

