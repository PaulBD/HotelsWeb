using Couchbase;
using System.Configuration;

namespace library.couchbase
{
    public class CouchBaseHelper
    {
        private string _bucketName;

        public CouchBaseHelper()
        {
            _bucketName = ConfigurationManager.AppSettings["couchbase.bucketName"];
        }

        /// <summary>
        /// Check record exists in couchbase
        /// </summary>
        public string CheckRecordExistsInDB(string key)
        {
            using (var cluster = new Cluster(CouchbaseConfig.Initialize()))
            {
                using (var bucket = cluster.OpenBucket(_bucketName))
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
        public dynamic ReturnQuery<T>(string query)
        {
            using (var cluster = new Cluster(CouchbaseConfig.Initialize()))
            {
                using (var bucket = cluster.OpenBucket(ConfigurationManager.AppSettings["couchbase.bucketName"]))
                {
                    var queryRequest = new Couchbase.N1QL.QueryRequest().Statement(query);

                    return bucket.Query<T>(queryRequest);
                }
            }
        }

        /// <summary>
        /// Add record to couchbase
        /// </summary>
        public void AddRecordToCouchbase(string key, string content, string factualId)
        {
            using (var cluster = new Cluster(CouchbaseConfig.Initialize()))
            {
                using (var bucket = cluster.OpenBucket(_bucketName))
                {
                    bucket.Insert(key, content);
                }
            }
        }
    }
}
