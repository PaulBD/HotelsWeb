﻿using System.Collections.Generic;
using core.deals.dtos;
using library.couchbase;

namespace core.deals.Services
{
    public class TravelzooService : ITravelzooService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooDeals";
        private string _query;

        public TravelzooService()
        {
            _couchbaseHelper = new CouchBaseHelper();
            _query = "SELECT aw_deep_link, product_name, merchant_image_url, description, search_price, currency FROM " + _bucketName;
        }

        public List<TravelzooDto> ReturnDeals(string location)
        {
            var q = _query + " WHERE location = '" + location + "'";

            return ProcessQuery(q);
        }

        public List<TravelzooDto> ReturnDealsExcludeLocation(string location)
        {
            var q = _query + " WHERE location != '" + location + "'";

            return ProcessQuery(q);
        }

        public List<TravelzooDto> ReturnTopDeals()
        {
            var q = _query + " LIMIT 3";

            return ProcessQuery(q);
        }

        /// <summary>
        /// Process Query
        /// </summary>
        private List<TravelzooDto> ProcessQuery(string q)
        {
            var result = _couchbaseHelper.ReturnQuery<TravelzooDto>(q, _bucketName);

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }
    }
}
