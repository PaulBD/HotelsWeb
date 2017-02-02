using System.Collections.Generic;

namespace core.places.dtos
{
    public class Address
    {
        public string address { get; set; }
        public string locality { get; set; }
        public string region { get; set; }
        public string postcode { get; set; }
        public List<string> neighborhood { get; set; }
        public string country { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string website { get; set; }
        public string adminRegion { get; set; }
        public string postTown { get; set; }
    }


    public class PlaceDto
    {
        public string id { get; set; }
        public string reference { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string hoursDisplay { get; set; }
        public object chainName { get; set; }
        public Address address { get; set; }
        public HoursDto hours { get; set; }
        public Coordinates coordinates { get; set; }
        public List<List<string>> tags { get; set; }
        public int customerRating { get; set; }
        public object mainImage { get; set; }
        public object images { get; set; }
    }
}
