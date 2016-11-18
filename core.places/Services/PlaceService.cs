using core.places.dtos;
using FactualDriver;
using System.Configuration;
using ServiceStack.Text;
using FactualDriver.Filters;
using library.couchbase;

namespace core.places.services
{
    public class PlaceService : IPlaceService
    {
        private Factual _factual;
        private CouchBaseHelper _couchbaseHelper;

        public PlaceService()
        {
            _factual = new Factual(ConfigurationManager.AppSettings["attractions.factual.key"], ConfigurationManager.AppSettings["attractions.factual.secret"]);

            _couchbaseHelper = new CouchBaseHelper();
        }

        /// <summary>
        /// Return a list of places by town and country
        /// </summary>
        public PlaceDto ReturnPlacesByTownAndCountry(string town, string country, string type, int limit, int offset)
        {
            var q = new Query();

            if (string.Compare(type, "all", true) != 0)
            {
                q.Search(type);
            }

            q.And(q.Field("locality").Equal(town));
            q.And(q.Field("country").Equal(country));

            return JsonSerializer.DeserializeFromString<PlaceDto>(ProcessQuery(q, limit, offset));
        }
        
        /// <summary>
        /// Return a list of places by proximity
        /// </summary>
        public PlaceDto ReturnPlacesByProximity(double longitude, double latitude, int radius, string type, int offset, int limit)
        {
            var q = new Query();

            if (string.Compare(type, "all", true) != 0)
            {
                q.Search(type);
            }

            q.WithIn(new Circle(longitude, latitude, radius));

            return JsonSerializer.DeserializeFromString<PlaceDto>(ProcessQuery(q, limit, offset));
        }

        /// <summary>
        /// Return a place by factual id
        /// </summary>
        public PlaceDto ReturnPlaceById(string factualId)
        {
            var result = "";

            // TODO: Return Cached Response

            var dbResult = _couchbaseHelper.CheckRecordExistsInDB("Place:" + factualId);

            if (dbResult == null)
            {
                result = _factual.FetchRow("places", factualId);
                _couchbaseHelper.AddRecordToCouchbase("Place:" + factualId, result, factualId);
            }
            else { result = dbResult; ; }

            // TODO: Add result to cache for next time

            return JsonSerializer.DeserializeFromString<PlaceDto>(result);
        }


        /// <summary>
        /// Process Query
        /// </summary>
        private string ProcessQuery(Query q, int limit, int offset)
        {
            if (limit > 0)
            {
                q.Limit(limit).Offset(offset);
            }

            return _factual.Fetch("places", q);
        }
    }
}
