using System;
using System.Threading.Tasks;
using TravelRecordApp.Helpers;
using Xamarin.Forms;
using Firebase.Auth;

[assembly: Dependency(typeof(TravelRecordApp.Droid.Dependencies.Auth))]
namespace TravelRecordApp.Droid.Dependencies
{
	public class Auth:IAuth
	{
		public Auth()
		{
		}

        public string GetCurrentUserId()
        {
            return FirebaseAuth.Instance.CurrentUser.Uid;
        }

        public bool IsAuthenticated()
        {
            return FirebaseAuth.Instance.CurrentUser != null;
        }

        public async Task<bool> LoginUser(string email, string password)
        {
            try
            {
                var result = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                if (result != null)
                {
                    var user = result.User;
                    return true;
                }
                else
                {
                    return false;
                }
               
            }
            catch(FirebaseAuthWeakPasswordException ex)
            {
                throw new Exception(ex.Message);
            }
            catch(FirebaseAuthInvalidUserException ex )
            {
                throw new Exception("There is no user registered");
            }
            catch (FirebaseAuthUserCollisionException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            try
            {
                await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch (FirebaseAuthWeakPasswordException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (FirebaseAuthInvalidUserException ex)
            {
                throw new Exception("There is no user registered");
            }
            catch (FirebaseAuthUserCollisionException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

