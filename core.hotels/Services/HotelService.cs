using library.couchbase;
using core.hotels.dtos;
using ServiceStack.Text;
using System.Collections.Generic;

namespace core.hotels.services
{
    public class HotelService : IHotelService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooHotels";

        public HotelService()
        {
            _couchbaseHelper = new CouchBaseHelper();
        }

        /// <summary>
        /// Return a list of hotels by town & country
        /// </summary>
        public List<HotelDto> ReturnHotelsByTown(string town, string country, int limit, int offset)
        {
            var q = "SELECT * FROM " + _bucketName + " WHERE LOWER(HotelCity) = '" + town.ToLower() + "' AND LOWER(HotelCountry) = '" + country.ToLower() + "'";
            
            return ProcessQuery(q, limit, offset);
        }

        /// <summary>
        /// Return a list of hotels by place id
        /// </summary>
        public List<HotelDto> ReturnHotelsByPlaceId(int placeId, int limit, int offset)
        {
            var q = "SELECT * FROM " + _bucketName + " WHERE PlaceId = " + placeId;

            return ProcessQuery(q, limit, offset);
        }

        /// <summary>
        /// Return a list of hotels by proximity
        /// </summary>
        public List<HotelDto> ReturnHotelsByProximity(double longitude, double latitude, int radius, int offset, int limit)
        {
            //TODO: Change query so its based upon latitude & longitude
            var q = "SELECT * FROM " + _bucketName + " WHERE LIMIT " + limit + " OFFSET " + offset;

            return ProcessQuery(q, limit, offset);
        }


        /// <summary>
        /// Process Query
        /// </summary>
        private List<HotelDto> ProcessQuery(string q, int limit, int offset)
        {
            if (limit > 0)
            {
                q += " LIMIT " + limit + " OFFSET " + offset;
            }

            return _couchbaseHelper.ReturnQuery<HotelDto>(q, _bucketName);
        }

        /// <summary>
        /// Return a single hotel By id
        /// </summary>
        public HotelDetailDto ReturnHotelById(string guid)
        {
            if (!guid.Contains("hotel"))
            {
                guid = "hotel:" + guid;
            }

            //TODO: Change query so its based upon latitude & longitude
            var q = "SELECT * FROM " + _bucketName + " WHERE Reference = '" + guid + "'";

            var result = ProcessQuery(q, 0, 0);

            if (result.Count > 0)
            {
                return result[0].TriperooHotels;
            }

            return null;
        }
    }
}
