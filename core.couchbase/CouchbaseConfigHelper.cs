using System;
using System.Configuration;

namespace library.couchbase
{
    public class CouchbaseConfigHelper
    {
        public CouchbaseConfigHelper()  { }

        private static CouchbaseConfigHelper instance = null;
        public static CouchbaseConfigHelper Instance
        {
            get { if (instance == null) { instance = new CouchbaseConfigHelper(); } return instance; }
        }

        public string Server
        {
            get
            {
                return ConfigurationManager.AppSettings["couchbase.domain"] + ":" + ConfigurationManager.AppSettings["couchbase.port"] + "/pools";
            }
        }

        public string BucketUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["couchbase.bucketUsername"];
            }
        }

        public string BucketPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["couchbase.bucketPassword"];
            }
        }

        public bool UseSSL
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["couchbase.useSSL"]);
            }
        }
    }
}