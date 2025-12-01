using System;
using System.Collections.Generic;
using TravelRecordApp.Helpers;

namespace TravelRecordApp.Model
{
	public class VenueRoot
	{
        public IList<Venue> results { get; set; }
        public Context context { get; set; }

        public VenueRoot()
		{
           
		}

        public static string GenerateUrl(double latitude, double longtitude)
        {
            string url = string.Format(Constants.VENUE_SEARCH, latitude, longtitude);
            return url;
        }
    }


    public class Category
    {
        public string fsq_category_id { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string plural_name { get; set; }
    }

    public class ExtendedLocation
    {
        public string dma { get; set; }
        public string census_block_id { get; set; }
    }

    public class Location
    {
        public string address { get; set; }
        public string locality { get; set; }
        public string region { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
        public string formatted_address { get; set; }
    }

    public class Parent
    {
        public string fsq_place_id { get; set; }
        public IList<Category> categories { get; set; }
        public string name { get; set; }
    }

    public class Child
    {
        public string fsq_place_id { get; set; }
        public IList<Category> categories { get; set; }
        public string name { get; set; }
    }

    public class RelatedPlaces
    {
        public Parent parent { get; set; }
        public IList<Child> children { get; set; }
    }

    public class SocialMedia
    {
        public string facebook_id { get; set; }
        public string instagram { get; set; }
        public string twitter { get; set; }
    }

    public class Chain
    {
        public string fsq_chain_id { get; set; }
        public string name { get; set; }
    }

    public class Venue
    {
        public string fsq_place_id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public IList<Category> categories { get; set; }
        public string date_created { get; set; }
        public string date_refreshed { get; set; }
        public int distance { get; set; }
        public ExtendedLocation extended_location { get; set; }
        public string link { get; set; }
        public Location location { get; set; }
        public string name { get; set; }
        public string placemaker_url { get; set; }
        public RelatedPlaces related_places { get; set; }
        public SocialMedia social_media { get; set; }
        public string tel { get; set; }
        public string website { get; set; }
        public string email { get; set; }
        public IList<Chain> chains { get; set; }
        public string store_id { get; set; }
    }

    public class Center
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Circle
    {
        public Center center { get; set; }
        public int radius { get; set; }
    }

    public class GeoBounds
    {
        public Circle circle { get; set; }
    }

    public class Context
    {
        public GeoBounds geo_bounds { get; set; }
    }



}

