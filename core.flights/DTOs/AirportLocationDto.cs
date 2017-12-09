using System;
using System.Collections.Generic;

namespace core.flights.dtos
{
    public class Country
    {
        public string id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string code { get; set; }
    }

    public class Region
    {
        public string id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

    public class Continent
    {
        public string id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string code { get; set; }
    }

    public class City
    {
        public string id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string slug { get; set; }
        public object subdivision { get; set; }
        public object autonomous_territory { get; set; }
        public Country country { get; set; }
        public Region region { get; set; }
        public Continent continent { get; set; }
    }

    public class Location2
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class Location
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public List<object> alternative_names { get; set; }
        public int rank { get; set; }
        public string timezone { get; set; }
        public City city { get; set; }
        public Location2 location { get; set; }
        public string type { get; set; }
    }

    public class Locale
    {
        public string code { get; set; }
        public string status { get; set; }
    }

    public class Meta
    {
        public Locale locale { get; set; }
    }

    public class AirportLocationDto
    {
        public AirportLocationDto()
        {
            locations = new List<Location>();
        }

        public List<Location> locations { get; set; }
        public Meta meta { get; set; }
    }
}

