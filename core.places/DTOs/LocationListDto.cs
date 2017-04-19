using System.Collections.Generic;

namespace core.places.dtos
{
    public class LocationDto
    {
        public int AverageReviewScore { get; set; }
        public string Doctype { get; set; }
        public string Image { get; set; }
        public int Latitude { get; set; }
        public string LetterIndex { get; set; }
        public int LikeCount { get; set; }
        public int ListingPriority { get; set; }
        public int Longitude { get; set; }
        public int ParentRegionID { get; set; }
        public string ParentRegionName { get; set; }
        public string ParentRegionNameLong { get; set; }
        public string ParentRegionType { get; set; }
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public string RegionNameLong { get; set; }
        public string RegionType { get; set; }
        public string RelativeSignificance { get; set; }
        public int ReviewCount { get; set; }
        public int SearchPriority { get; set; }
        public string SubClass { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
    }

    public class LocationListDto
    {
        public LocationListDto()
        {
            Locations = new List<LocationDto>();
        }

        public IList<LocationDto> Locations { get; set; }
        public int LocationCount { get; set; }
    }
}
