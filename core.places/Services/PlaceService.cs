using core.places.dtos;
using FactualDriver;
using library.couchbase;
using System.Collections.Generic;

namespace core.places.services
{
    public class PlaceService : IPlaceService
    {
        private Factual _factual;
        private CouchBaseHelper _couchbaseHelper;

        public PlaceService()
        {
            _couchbaseHelper = new CouchBaseHelper();
        }

        /// <summary>
        /// Return a list of places by town and country
        /// </summary>
        public List<FactualDto> ReturnPlacesByTownAndCountry(string town, string country, string type, int limit, int offset)
        {
            /*
            var result = _couchbaseHelper.ReturnQuery<FactualDto>("SELECT * FROM Triperoo where address.postTown = '" + town + "'", "TriperooCommon");

            if (result == null)
            {
                var q = new Query();

                if (string.Compare(type, "all", true) != 0)
                {
                    q.Search(type);
                }

                q.And(q.Field("locality").Equal(town));
                q.And(q.Field("country").Equal(country));

                return JsonSerializer.DeserializeFromString<List<FactualDto>>(ProcessQuery(q, limit, offset));
            }
            else { return result; }
            */
            return null;
        }
        
        /// <summary>
        /// Return a list of places by proximity
        /// </summary>
        public FactualDto ReturnPlacesByProximity(double longitude, double latitude, int radius, string type, int offset, int limit)
        {
            /*
            var q = new Query();

            if (string.Compare(type, "all", true) != 0)
            {
                q.Search(type);
            }

            q.WithIn(new Circle(longitude, latitude, radius));

            return JsonSerializer.DeserializeFromString<FactualDto>(ProcessQuery(q, limit, offset));
            */
            return null;
        }

        /// <summary>
        /// Return a place by factual id
        /// </summary>
        public PlaceDto ReturnPlaceById(string placeId, string type)
        {
            /*
            var result = _couchbaseHelper.CheckRecordExistsInDB("place:" + type + ":" + placeId, "TriperooCommon");

            return JsonSerializer.DeserializeFromString<PlaceDto>(result);
            */
            return null;
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
