using Couchbase;
using System.Collections.Generic;
using Couchbase.Configuration.Client;
using System;
using Couchbase.Authentication;
using System.Configuration;

namespace library.couchbase
{
    public class CouchBaseHelper
    {
        /// <summary>
        /// Check record exists in couchbase
        /// </summary>
        public string CheckRecordExistsInDB(string key, string bucketName)
        {
            var cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(ConfigurationManager.AppSettings["couchbase.domain"]) }
            });

            var authenticator = new PasswordAuthenticator(ConfigurationManager.AppSettings["couchbase.bucketUsername"], ConfigurationManager.AppSettings["couchbase.bucketPassword"]);
            cluster.Authenticate(authenticator);

            using (var bucket = cluster.OpenBucket(bucketName))
            {
                var item = bucket.GetDocument<dynamic>(key);

                if (item.Success)
                {
                    return item.Content;
                }
            }

            return null;
        }

        /// <summary>
        /// Return Dynamic Query
        /// </summary>
        public List<T> ReturnQuery<T>(string query, string bucketName)
        {
            var cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(ConfigurationManager.AppSettings["couchbase.domain"]) }
            });

            var authenticator = new PasswordAuthenticator(ConfigurationManager.AppSettings["couchbase.bucketUsername"], ConfigurationManager.AppSettings["couchbase.bucketPassword"]);
            cluster.Authenticate(authenticator);

            using (var bucket = cluster.OpenBucket(bucketName))
            {
                var queryRequest = new Couchbase.N1QL.QueryRequest().Statement(query);

                var result = bucket.Query<T>(queryRequest);

                return result.Rows;
            }
        }

        /// <summary>
        /// Add record to couchbase
        /// </summary>
        public void AddRecordToCouchbase(string key, string content, string factualId, string bucketName)
        {
            var cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(ConfigurationManager.AppSettings["couchbase.domain"]) }
            });

            var authenticator = new PasswordAuthenticator(ConfigurationManager.AppSettings["couchbase.bucketUsername"], ConfigurationManager.AppSettings["couchbase.bucketPassword"]);
            cluster.Authenticate(authenticator);

            using (var bucket = cluster.OpenBucket(bucketName))
            {
                bucket.Upsert(key, content);
            }
        }

        /// <summary>
        /// Add record to couchbase
        /// </summary>
        public void AddRecordToCouchbase(string key, object json, string bucketName)
        {
            var cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(ConfigurationManager.AppSettings["couchbase.domain"]) }
            });

            var authenticator = new PasswordAuthenticator(ConfigurationManager.AppSettings["couchbase.bucketUsername"], ConfigurationManager.AppSettings["couchbase.bucketPassword"]);
            cluster.Authenticate(authenticator);

            using (var bucket = cluster.OpenBucket(bucketName))
            {
                bucket.Upsert(key, json);
            }
		}
    }
}
