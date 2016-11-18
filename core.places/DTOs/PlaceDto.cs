using System.Collections.Generic;

namespace core.places.dtos
{
    public class HoursDto
    {
        public List<List<string>> Sunday { get; set; }
        public List<List<string>> Saturday { get; set; }
        public List<List<string>> Tuesday { get; set; }
        public List<List<string>> Friday { get; set; }
        public List<List<string>> Thursday { get; set; }
        public List<List<string>> Wednesday { get; set; }
        public List<List<string>> Monday { get; set; }
    }
    
    public class DataDto
    {
        public string Factual_Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Address_Extended { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public List<string> Neighborhood { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Hours_Display { get; set; }
        public HoursDto Hours { get; set; }
        public string Admin_Region { get; set; }
        public string Post_Town { get; set; }
        public string Chain_Name { get; set; }
        public string Chain_Id { get; set; }
        public List<List<string>> Category_Labels { get; set; }
        public List<int> Category_Ids { get; set; }
        public string Email { get; set; }
        public string Po_Box { get; set; }
        public int ScoreOutOf6 { get; set; }
        public string MainImage { get; set; }
        public List<string> Images { get; set; }
    }

    public class ResponseDto
    {
        public List<DataDto> Data { get; set; }
        public int Count { get { return Data.Count; } }
    }

    public class PlaceDto
    {
        public string Type { get { return "Place"; } }
        public int Version { get; set; }
        public string Status { get; set; }
        public ResponseDto Response { get; set; }
    }
}
