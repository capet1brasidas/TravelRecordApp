using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TravelRecordApp.Helpers;
using TravelRecordApp.Model;

namespace TravelRecordApp.Logic
{
	public class VenueLogic
	{
		public VenueLogic()
		{
			
		}

        public async static Task<List<Venue>> GetVenues(double latitude, double longitude)
        {
            List<Venue> venues = new List<Venue>();

            var url = VenueRoot.GenerateUrl(latitude, longitude);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Constants.BEARER_TOKEN);

                client.DefaultRequestHeaders.Add(Constants.VERSION_HEADER, Constants.VERSION_VALUE);

                var response =await client.GetAsync(url);
          

                var json =await response.Content.ReadAsStringAsync();

                var venueRoot = JsonConvert.DeserializeObject<VenueRoot>(json);

                venues = venueRoot.results as List<Venue>;
            }



            return venues;
        }
    }
}

