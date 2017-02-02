using System.Net;
using ServiceStack;

namespace triperoo.apis.endpoints.hotels
{
    #region Flush Cache

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/cache/clear")]
    public class CacheRequest
    {
    }

    #endregion

    #region API logic

    public class CacheApi : Service
    {
        /// <summary>
        /// Flush the cache
        /// </summary>
        public object Get(CacheRequest request)
        {
            Cache.FlushAll();

            return new HttpResult(HttpStatusCode.OK);
        }
    }

    #endregion
}
