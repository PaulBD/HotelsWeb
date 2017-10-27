using System;
using System.Collections.Generic;

namespace core.places.dtos
{

	public class SourceData
	{
		public string ForesquareId { get; set; }
	}

	public class ContactDetails
	{
		public string phone { get; set; }
		public string formattedPhone { get; set; }
		public string twitter { get; set; }
		public string facebook { get; set; }
		public string facebookUsername { get; set; }
		public string facebookName { get; set; }
		public string instagram { get; set; }
		public string websiteUrl { get; set; }
	}

	public class PhotoList
	{
		public string prefix { get; set; }
		public string suffix { get; set; }
		public int width { get; set; }
		public int height { get; set; }
        public string customerReference { get; set; }
        public string photoReference { get; set; }
	}

	public class Photos
	{
        public Photos()
        {
            PhotoList = new List<PhotoList>();
        }
        public int PhotoCount { get { if (PhotoList != null) { return PhotoList.Count; } else { return 0; } } }
		public List<PhotoList> PhotoList { get; set; }
	}

	public class LocationCoordinatesDto
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
	}

	public class SummaryDto
	{
		public string en { get; set; }
        public string fr { get; set; }
		public string de { get; set; }
		public string es { get; set; }
		public string it { get; set; }
	}

	public class StatsDto
	{
		public int LikeCount { get; set; }
		public int ReviewCount { get; set; }
		public double AverageReviewScore { get; set; }
	}

    public class Style
    {
        public string text { get; set; }
    }

    public class Styles
    {
        public List<Style> style { get; set; }
    }

    public class AveragePriceThreeCourseMeal
    {
        public string currency { get; set; }
        public decimal text { get; set; }
    }

    public class AveragePriceMainCourse
    {
        public string currency { get; set; }
        public decimal text { get; set; }
    }

    public class Pricing
    {
        public string PriceAUD { get; set; }
        public string PriceNZD { get; set; }
        public string PriceEUR { get; set; }
        public string PriceGBP { get; set; }
        public string PriceUSD { get; set; }
        public string PriceCAD { get; set; }
        public string PriceCHF { get; set; }
        public string PriceNOK { get; set; }
        public string PriceJPY { get; set; }
        public string PriceSEK { get; set; }
        public string PriceHKD { get; set; }
        public string PriceSGD { get; set; }
        public string PriceZAR { get; set; }
    }

	public class LocationDetail
	{
		public Styles styles { get; set; }
		public string openHours { get; set; }
		public AveragePriceThreeCourseMeal averagePriceThreeCourseMeal { get; set; }
		public AveragePriceMainCourse averagePriceMainCourse { get; set; }

        public string bookingUrl { get; set; }
        public string productCode { get; set; }
        public string productType { get; set; }
        public string duration { get; set; }
        public string iataCode { get; set; }
        public string bookingType { get; set; }
        public string voucherOption { get; set; }
        public string productRating { get; set; }
        public string productRatingUrl { get; set; }
        public Pricing pricing { get; set; }
	}

    public class SuggestedActivity
    {
        public string regionName { get; set; }
        public string regionUrl { get; set; }
    }

	public class LocationDto
    {
        public LocationDto()
        {
            SourceData = new SourceData();
            FormattedAddress = new List<string>();
            Tags = new List<string>();
            ContactDetails = new ContactDetails();
            Photos = new Photos();
            Summary = new SummaryDto();
            Stats = new StatsDto();
            LocationCoordinates = new LocationCoordinatesDto();
            LocationDetail = new LocationDetail();
        }

        public LocationDetail LocationDetail { get; set; }
        public string Doctype { get; set; }
        public string Image { get; set; }
        public double? Latitude { get; set; }
        public string LetterIndex { get; set; }
        public int? ListingPriority { get; set; }
        public double? Longitude { get; set; }
        public int ParentRegionID { get; set; }
        public string ParentRegionName { get; set; }
        public string ParentRegionImage { get; set; }
        public string ParentRegionNameLong { get; set; }
        public string ParentRegionType { get; set; }
        public Int64 RegionID { get; set; }
		public string RegionName { get; set; }
        public string CountryCode { get; set; }
		public string StateProvinceCode { get; set; }
        public string RegionNameLong { get; set; }
        public string RegionType { get; set; }
        public string RelativeSignificance { get; set; }
        public int? SearchPriority { get; set; }
        public string SubClass { get; set; }
        public SummaryDto Summary { get; set; }
        public string Url { get; set; }
        public SuggestedActivity suggestedActivity { get; set; }
        public string ParentUrl
        {
            get
            {
                if ((ParentRegionID > 0) && (!string.IsNullOrEmpty(ParentRegionNameLong)))
                {
                    return "/" + ParentRegionID + "/visit/" + ParentRegionNameLong.Replace("(", "").Replace(")", "").Replace(",", "").Replace(" ", "-").Replace("&", "and").ToLower();
                }
                else { return null; }
            }
        }

        public SourceData SourceData { get; set; }
		public List<string> FormattedAddress { get; set; }
		public List<string> Tags { get; set; }
		public ContactDetails ContactDetails { get; set; }
		public Photos Photos { get; set; }
		public StatsDto Stats { get; set; }
		public LocationCoordinatesDto LocationCoordinates { get; set; }
	}

    public class LocationListDto
    {
        public LocationListDto()
        {
			Locations = new List<LocationDto>();
			MapLocations = new List<MapLocationDto>();
            Categories = new List<CategoryDto>();
        }

		public IList<LocationDto> Locations { get; set; }
		public IList<MapLocationDto> MapLocations { get; set; }
		public int LocationCount { get; set; }
        public IList<CategoryDto> Categories { get; set; }
    }

    public class MapLocationDto
	{
		public MapLocationDto()
		{
			LocationCoordinates = new LocationCoordinatesDto();
		}

		public string RegionName { get; set; }
		public string SubClass { get; set; }
		public LocationCoordinatesDto LocationCoordinates { get; set; }
		public string Url { get; set; }
		public string Image { get; set; }
    }
}
