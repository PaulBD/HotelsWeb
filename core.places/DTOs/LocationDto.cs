using System.Collections.Generic;

namespace core.places.dtos
{
    public class CityPlace
    {
        public string url { get; set; }
        public string name { get; set; }
        public int count { get; set; }
    }

    public class CityData
    {
        public CityData()
        {
            hotels = new CityPlace() { count = 0, name = "Hotels", url = "/hotels" };
            restaurants = new CityPlace() { count = 0, name = "Restaurants", url = "/restaurants" };
            nightlife = new CityPlace() { count = 0, name = "Nightlife", url = "/nightlife" };
            attractions = new CityPlace() { count = 0, name = "Attractions", url = "/attractions" };
            reviews = new CityPlace() { count = 0, name = "Reviews", url = "/reviews" };
            questions = new CityPlace() { count = 0, name = "Questions", url = "/questions" };
        }

        public CityPlace hotels { get; set; }
        public CityPlace restaurants { get; set; }
        public CityPlace nightlife { get; set; }
        public CityPlace attractions { get; set; }
        public CityPlace reviews { get; set; }
        public CityPlace questions { get; set; }
    }

    public class Text
    {
        public object de { get; set; }
        public object en { get; set; }
        public object es { get; set; }
        public object fr { get; set; }
        public object it { get; set; }
        public object us { get; set; }
    }

    public class LocationPlace
    {
        public int count { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }

    public class Statistics
    {
        public int reviewCount { get; set; }
        public int visitorCount { get; set; }
    }

    public class LocationDetail
    {
        public LocationDetail()
        {
            coordinates = new Coordinates();
            description = new Text();
            name = new Text();
            places = new List<LocationPlace>();
            statistics = new Statistics();
            data = new CityData();
        }

        public Coordinates coordinates { get; set; }
        public string country { get; set; }
        public Text description { get; set; }
        public int id { get; set; }
        public string imageUrl { get; set; }
        public Text name { get; set; }
        public string parentName { get; set; }
        public List<LocationPlace> places { get; set; }
        public string reference { get; set; }
        public string region { get; set; }
        public Statistics statistics { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public CityData data { get; set; }
    }

    public class LocationDto
    {
        public LocationDetail TriperooCommon { get; set; }
    }
}
