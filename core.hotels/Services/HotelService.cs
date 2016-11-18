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
            var list = new HotelListDto();
            var q = "SELECT * FROM " + CouchbaseConfigHelper.Instance.BucketName + " WHERE HotelCity = '" + town + "' AND HotelCountry = '" + country + "'";

            //TODO: Add limit & Offset to query

            var item = _couchbaseHelper.ReturnQuery<HotelListDto>(q);
            
            //TODO: Fix this
            if (item.Success)
            {
                list.HotelList.AddRange(item.Rows);
            }

            return list;
        }

        /// <summary>
        /// Return a list of hotels by proximity
        /// </summary>
        public HotelListDto ReturnHotelsByProximity(double longitude, double latitude, int radius, int offset, int limit)
        {
            //TODO: Change query so its based upon latitude & longitude
            var list = new HotelListDto();
            var q = "SELECT * FROM " + CouchbaseConfigHelper.Instance.BucketName + " WHERE ";

            //TODO: Add limit & Offset to query

            var item = _couchbaseHelper.ReturnQuery<HotelListDto>(q);

            //TODO: Fix this
            if (item.Success)
            {
                list.HotelList.AddRange(item.Rows);
            }

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
