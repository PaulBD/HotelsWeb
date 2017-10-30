using System;
using System.Collections.Generic;
using System.Linq;
using core.places.dtos;
using library.couchbase;

namespace core.places.services
{
    public class BaseService
	{
		private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooCommon";

        public BaseService()
		{
			_couchbaseHelper = new CouchBaseHelper();
        }

		/// <summary>
		/// Process Query
		/// </summary>
		protected LocationListDto ProcessQuery(string q)
		{
			var list = new LocationListDto();
			var result = _couchbaseHelper.ReturnQuery<LocationDto>(q, _bucketName);

			if (result.Count > 0)
			{
                list.Locations = result;
				list.Categories = result.GroupBy(r => r.SubClass, (subClass, group) => new { subClass, count = group.Count() }).Select(g => new CategoryDto
				{
					Count = g.count,
                    CategoryNameFriendly = ReturnFriendlyName(g.subClass),
                    CategoryName = g.subClass.Replace(" & ", "-and-").Replace(",", "0").Replace(" ", "-").ToLower()
				}).ToList();

                list.Categories = list.Categories.OrderByDescending(w => w.Count).ToList();

				// Copy the map coordinates
				list.MapLocations = result.Select(l => new MapLocationDto
				{
					RegionName = l.RegionName,
					LocationCoordinates = l.LocationCoordinates,
					Url = l.Url,
					SubClass = l.SubClass,
					Image = l.Image
				}).ToList();
			}

			return list;
		}

        private string ReturnFriendlyName(string value)
        {
            switch (value)
            {
                case "tree":
                    return "Park / Common";
                case "sign":
                    return "Area of Interest";
                case "civic":
                    return "Important Buildings";
                case "anchor":
                    return "Docklands";
                case "icecream":
                    return "Activities";
                case "stadium":
                    return "Sport Venues";
                case "medical":
                    return "Hospitals, Medical Buildings";
                case "school":
                    return "Schools, Colleages, Universities";
                case "theater":
                    return "Theatres";
                case "historic":
                    return "Historic Venues";
                case "neighbor":
                    return "";
            }
            return value;
        }
    }
}
