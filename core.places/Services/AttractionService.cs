using System;
using core.places.dtos;
using library.caching;

namespace core.places.services
{
    public class AttractionService: BaseService, IAttractionService
    {
        private readonly string _bucketName = "TriperooAttractions";
        private string _query;
        private readonly ICacheProvider _cache;
        private readonly ILocationService _locationService;

        public AttractionService(ICacheProvider cache, ILocationService locationService)
        {
            _cache = cache;
            _locationService = locationService;
            _query = "SELECT BookingType, Commences, Destination,Duration,Introduction,Pricing, ProductCategories,ProductCode,ProductImage,ProductName,ProductStarRating, ProductText,ProductType,ProductURLs,Rank,Special,SpecialDescription,VoucherOption FROM " + _bucketName;
        }


        public AttractionListDto ReturnAttractionsById(int id)
        {
            var location = _locationService.ReturnLocationById(id);

            if (location != null)
            {
                var locList = location.RegionNameLong.Split(',');

                var country = locList[locList.Length - 1].Trim();

                var cacheKey = "POI:" + location.RegionName.Replace(" ", "_").ToLower() + country.Replace(" ", "_").ToLower();

                var attractionsList = _cache.Get<AttractionListDto>(cacheKey);

                if (attractionsList != null)
                {
                    //return attractionsList;
                }

                var q = _query + " WHERE Destination.City = '" + location.RegionName + "' AND Destination.Country = '" + country + "' ORDER BY Rank";

                /*var list = null;//ProcessAttractionQuery(q);

                if (list != null)
                {
                    _cache.AddOrUpdate(cacheKey, list);
                }
                */

                //return list;
            }

            return null;
        }
    }
}
