using library.couchbase;
using core.hotels.dtos;
using ServiceStack.Text;

namespace core.hotels.services
{
    public class HotelService : IHotelService
    {
        private CouchBaseHelper _couchbaseHelper;

        public HotelService()
        {
            _couchbaseHelper = new CouchBaseHelper();
        }

        /// <summary>
        /// Return a list of hotels by town & country
        /// </summary>
        public HotelListDto ReturnHotelByTown(string town, string country, int limit, int offset)
        {
            var q = "SELECT * FROM " + CouchbaseConfigHelper.Instance.BucketName + " WHERE HotelCity = '" + town + "' AND HotelCountry = '" + country + "'";
            
            return ProcessQuery(q, limit, offset);
        }

        /// <summary>
        /// Return a list of hotels by proximity
        /// </summary>
        public HotelListDto ReturnHotelsByProximity(double longitude, double latitude, int radius, int offset, int limit)
        {
            //TODO: Change query so its based upon latitude & longitude
            var q = "SELECT * FROM " + CouchbaseConfigHelper.Instance.BucketName + " WHERE LIMIT " + limit + " OFFSET " + offset;

            return ProcessQuery(q, limit, offset);
        }


        /// <summary>
        /// Process Query
        /// </summary>
        private HotelListDto ProcessQuery(string q, int limit, int offset)
        {
            var list = new HotelListDto();

            if (limit > 0)
            {
                q += " LIMIT " + limit + " OFFSET " + offset;
            }

            var item = _couchbaseHelper.ReturnQuery<HotelDto>(q);

            list.HotelList.AddRange(item);

            return list;
        }

        /// <summary>
        /// Return a single hotel By id
        /// </summary>
        public HotelDetailsDto ReturnHotelById(string id)
        {
            var result = _couchbaseHelper.CheckRecordExistsInDB("Hotels:" + id);

            return JsonSerializer.DeserializeFromString<HotelDetailsDto>(result);
        }
        
    }
}
