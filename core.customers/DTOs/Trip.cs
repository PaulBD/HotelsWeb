using System;
using System.Collections.Generic;

namespace core.customers.dtos
{
    public class TripDetails
    {
        public TripDetails()
        {
            TripSummary = new List<TripSummary>();
        }

        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public string RegionUrl { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string TripStart { get; set; }
        public string TripEnd { get; set; }
        public List<TripSummary> TripSummary { get; set; }
        public double TripLength { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public string Type { get; set; }
        public string TripPace { get; set; }
        public string PopulateTrip { get; set; }
        public List<string> Tags { get; set; }
    }

    public class TripSummary 
    {
        public int Day { get; set; }
        public string Date { get; set; }
        public int Count { get; set; }
        public int TotalDuration { get; set; }
        
    }

    public class ActivityDto
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public string VisitDate { get; set; }
        public string Type { get; set; }
        public int StartTime { get; set; }
        public string StartTimePeriod { get; set; }
        public string Price { get; set; }
        public string Length { get; set; }
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public string RegionNameLong { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
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
        public string Type { get { return "trip"; } }
        public string TripName { get; set; }
        public bool IsArchived { get; set; }
        public TripDetails TripDetails { get; set; }
        public List<ActivityDto> Days { get; set; }
    }
}
