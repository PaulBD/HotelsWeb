using Couchbase;
using System.Collections.Generic;

namespace library.couchbase
{
    public class CouchBaseHelper
    {
        /// <summary>
        /// Check record exists in couchbase
        /// </summary>
        public string CheckRecordExistsInDB(string key, string bucketName)
        {
            using (var cluster = new Cluster(CouchbaseConfig.Initialize(bucketName)))
            {
                using (var bucket = cluster.OpenBucket(bucketName))
                {
                    var item = bucket.GetDocument<dynamic>(key);

                    if (item.Success)
                    {
                        return item.Content;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Return Dynamic Query
        /// </summary>
        public List<T> ReturnQuery<T>(string query, string bucketName)
        {
            using (var cluster = new Cluster(CouchbaseConfig.Initialize(bucketName)))
            {
                using (var bucket = cluster.OpenBucket(bucketName))
                {
                    var queryRequest = new Couchbase.N1QL.QueryRequest().Statement(query);

                    return bucket.Query<T>(queryRequest).Rows;
                }
            }
        }

        /// <summary>
        /// Add record to couchbase
        /// </summary>
        public void AddRecordToCouchbase(string key, string content, string factualId, string bucketName)
        {
            using (var cluster = new Cluster(CouchbaseConfig.Initialize(bucketName)))
            {
                using (var bucket = cluster.OpenBucket(bucketName))
                {
                    bucket.Upsert(key, content);
                }
            }
        }

        /// <summary>
        /// Add record to couchbase
        /// </summary>
        public void AddRecordToCouchbase(string key, object json, string bucketName)
        {
            using (var cluster = new Cluster(CouchbaseConfig.Initialize(bucketName)))
            {
                using (var bucket = cluster.OpenBucket(bucketName))
                {
                    bucket.Upsert(key, json);
                }
            }
		}
    }
}
