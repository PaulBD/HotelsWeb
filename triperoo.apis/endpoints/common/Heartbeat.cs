using System.Net;
using ServiceStack;

namespace triperoo.apis.endpoints.hotels
{
    #region Return Heartbeat

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/heartbeat")]
    public class HeartbeatRequest
    {
    }

    #endregion

    #region API logic

    public class HeartbeatApi : Service
    {
        /// <summary>
        /// Return 200 for heartbeat
        /// </summary>
        public object Get(HeartbeatRequest request)
        {
            return new HttpResult(HttpStatusCode.OK);
        }
    }

    #endregion
}
