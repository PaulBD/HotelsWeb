using System.Collections.Generic;
using System.Web;

namespace library.foursquare.dtos
{
    public class Meta
	{
		public int Code { get; set; }
		public string RequestId { get; set; }
	}

	public class Contact
	{
		public string Phone { get; set; }
		public string FormattedPhone { get; set; }
		public string Twitter { get; set; }
		public string Instagram { get; set; }
		public string Facebook { get; set; }
		public string FacebookUsername { get; set; }
		public string FacebookName { get; set; }
	}

	public class LabeledLatLng
	{
		public string Label { get; set; }
		public double Lat { get; set; }
		public double Lng { get; set; }
	}

	public class Location
	{
        public Location(){
            FormattedAddress = new List<string>();
            LabeledLatLngs = new List<LabeledLatLng>();
        }

		public string Address { get; set; }
		public double Lat { get; set; }
		public double Lng { get; set; }
		public string PostalCode { get; set; }
		public string CC { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public List<string> FormattedAddress { get; set; }
		public List<LabeledLatLng> LabeledLatLngs { get; set; }
		public string CrossStreet { get; set; }
	}

	public class Icon
	{
		public string Prefix { get; set; }
		public string Suffix { get; set; }
	}

	public class Category
	{
        public Category() {
            Icon = new Icon();
        }

		public string Id { get; set; }
		public string Name { get; set; }
		public string PluralName { get; set; }
		public string ShortName { get; set; }
		public Icon Icon { get; set; }
		public bool Primary { get; set; }
	}

	public class Stats
	{
		public int CheckinsCount { get; set; }
		public int UsersCount { get; set; }
		public int TipCount { get; set; }
	}

	public class BeenHere
	{
		public int LastCheckinExpiredAt { get; set; }
	}

	public class Specials
	{
        public Specials() {
            Items = new List<object>();
        }

		public int Count { get; set; }
		public List<object> Items { get; set; }
	}

	public class HereNow
	{
        public HereNow(){
            Groups = new List<object>();
        }
		public int Count { get; set; }
		public string Summary { get; set; }
		public List<object> Groups { get; set; }
	}

	public class VenuePage
	{
		public string id { get; set; }
	}

	public class Venue
	{
        public Venue() {
            Categories = new List<Category>();
            Location = new Location();
            Contact = new Contact();
            Stats = new Stats();
            BeenHere = new BeenHere();
            Specials = new Specials();
            HereNow = new HereNow();
            VenueChains = new List<object>();
            VenuePage = new VenuePage();
        }
		public string Id { get; set; }
		public string Name { get; set; }
		public Contact Contact { get; set; }
		public Location Location { get; set; }
		public List<Category> Categories { get; set; }
		public bool Verified { get; set; }
		public Stats Stats { get; set; }
		public string Url { get; set; }
		public BeenHere BeenHere { get; set; }
		public Specials Specials { get; set; }
		public string StoreId { get; set; }
		public HereNow HereNow { get; set; }
		public string ReferralId { get; set; }
		public List<object> VenueChains { get; set; }
		public bool HasPerk { get; set; }
		public bool? VenueRatingBlacklisted { get; set; }
		public bool? AllowMenuUrlEdit { get; set; }
		public VenuePage VenuePage { get; set; }
	}

	public class Coordinates
	{
		public double Lat { get; set; }
		public double Lng { get; set; }
	}

	public class Geometry
	{
        public Geometry()
        {
            Center = new Coordinates();
            Bounds = new Coordinates();
        }

		public Coordinates Center { get; set; }
		public Coordinates Bounds { get; set; }
	}

	public class Feature
	{
        public Feature()
        {
            Geometry = new Geometry();
        }

		public string CC { get; set; }
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public string MatchedName { get; set; }
		public string HighlightedName { get; set; }
		public int WoeType { get; set; }
		public string Slug { get; set; }
		public string Id { get; set; }
		public string LongId { get; set; }
		public Geometry Geometry { get; set; }
	}

	public class Geocode
	{
        public Geocode() {
            Parents = new List<object>();
            Feature = new Feature();
        }

		public string What { get; set; }
		public string Where { get; set; }
		public Feature Feature { get; set; }
		public List<object> Parents { get; set; }
	}

	public class Response
	{
        public Response()
        {
            Venues = new List<Venue>();
            Geocode = new Geocode();
        }

		public List<Venue> Venues { get; set; }
		public Geocode Geocode { get; set; }
	}

	public class VenueDto
	{
		public Meta Meta { get; set; }
		public Response Response { get; set; }
	}
}
