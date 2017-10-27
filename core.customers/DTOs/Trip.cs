using System;
using System.Collections.Generic;

namespace core.customers.dtos
{
    public class TripDetails
    {
        public int regionID { get; set; }
        public string regionName { get; set; }
        public string regionUrl { get; set; }
        public string image { get; set; }
        public int tripStart { get; set; }
        public int tripEnd { get; set; }
        public int tripLength { get; set; }
        public int adults { get; set; }
        public int children { get; set; }
        public string type { get; set; }
        public string tripPace { get; set; }
        public List<string> tags { get; set; }
    }

    public class ActivityDto
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public string Type { get; set; }
        public int StartTime { get; set; }
        public int Length { get; set; }
        public string LengthValue { get; set; }
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public string RegionUrl { get; set; }
        public string TravelType { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsArchived { get; set; }
    }

    public class TripDto
    {
        public TripDto()
        {
            TripDetails = new TripDetails();
            Days = new List<ActivityDto>();
        }

        public int Id { get; set; }
        public string CustomerReference { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string TripName { get; set; }
        public bool IsArchived { get; set; }
        public TripDetails TripDetails { get; set; }
        public List<ActivityDto> Days { get; set; }
    }
}
