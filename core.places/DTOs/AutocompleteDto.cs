namespace core.places.dtos
{
    public class AutocompleteDto
    {
        private string _regionName;
        private string _regionNameLong;
        private string _parentRegionName;
        public AutocompleteDto()
        {
            LocationCoordinates = new LocationCoordinates();
            Summary = new Summary();
            Stats = new Stats();
        }

        public string Doctype { get; set; }
        public int RegionID { get; set; }
        public string RegionType { get; set; }
        public string RelativeSignificance { get; set; }
        public string SubClass { get; set; }
        public string RegionName
        {
            get
            {
                return _regionName.Replace(" (county)", "");
            }

            set
            {
                _regionName = value;
            }
        }
        public string RegionNameLong
        {
            get
            {
                return _regionNameLong.Replace(" (county)", "");
            }

            set
            {
                _regionNameLong = value;
            }
        }
        public int ParentRegionID { get; set; }
        public string ParentRegionType { get; set; }
        public string ParentRegionName
        {
            get
            {
                return _parentRegionName.Replace(" (county)", "");
            }

            set
            {
                _parentRegionName = value;
            }
        }
        public string ParentRegionNameLong { get; set; }
        public string LetterIndex
        {
            get
            {
                return RegionNameLong.Substring(0, 3).ToLower();
            }
        }

        public LocationCoordinates LocationCoordinates { get; set; }
        public Summary Summary { get; set; }
        public Stats Stats { get; set; }

        public string Url
        {
            get
            {
                if (RegionType.Contains("Interest"))
                {
                    var name = RegionNameLong.ToLower().Trim().Replace(",", "").Replace(". ", "-").Replace("'", "-").Replace(" - ", "-").Replace(" ", "-").Replace("(", "").Replace(")", "");
                    return "/" + RegionID + "/visit-location/" + name;
                }
                else
                {
                    var name = RegionNameLong.ToLower().Trim().Replace(",", "").Replace(". ", "-").Replace("'", "-").Replace(" - ", "-").Replace(" ", "-").Replace("(", "").Replace(")", "");
                    return "/" + RegionID + "/visit/" + name;
                }
            }
        }

        public string Image
        {
            get
            {
                var name = RegionNameLong.ToLower().Trim().Replace(",", "").Replace(". ", "-").Replace("'", "-").Replace(" - ", "-").Replace(" ", "-").Replace("(", "").Replace(")", "");
                return "/static/img/locations/" + name + ".png";
            }
        }

        public int SearchPriority
        {
            get
            {
                switch (RegionType)
                {
                    case "Continent":
                        return 1;
                    case "Country":
                        return 2;
                    case "Multi - Region(within a country)":
                        return 3;
                    case "Multi-Region (within a country)":
                        return 3;
                    case "Province (State)":
                        return 5;
                    case "City":
                        return 6;
                    case "Multi-City (Vicinity)":
                        return 7;
                    case "Point of Interest":
                    case "Point of Interest Shadow":
                        return 8;
                }

                return 0;
            }
        }

        public int ListingPriority { get; set; }
    }


    public class LocationCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Summary
    {
        public string En { get; set; }
        public string Fr { get; set; }
        public string De { get; set; }
        public string Es { get; set; }
        public string It { get; set; }
    }


    public class Stats
    {
        public int LikeCount { get; set; }
        public int ReviewCount { get; set; }
        public double AverageReviewScore { get; set; }
    }

}
