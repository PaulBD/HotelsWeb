using ServiceStack;
using System;
using System.Net;

namespace triperoo.apis.endpoints.customer
{
    #region Cache Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/cache/clear", "GET")]
    public class CacheRequest
    {
    }

    #endregion

    #region API logic

    public class CacheApi : Service
    {
        #region Clear Cache

        /// <summary>
        /// Clear Cache
        /// </summary>
        public object Get(CacheRequest request)
        {
            try
            {
                base.Cache.ClearCaches();
                base.Cache.ClearSession();
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(HttpStatusCode.OK);
        }

        #endregion
    }

    #endregion
}
