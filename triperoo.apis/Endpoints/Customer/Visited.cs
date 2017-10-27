using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using System;
using System.Net;

namespace triperoo.apis.endpoints.customer
{
    #region Visit Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/customer/visit", "POST")]
    public class VisitRequest
    {
        public CustomerLocationDto Location { get; set; }
    }

    #endregion

    #region API logic

    public class VisitApi : Service
    {
        private readonly IVisitedService _visitedService;

        /// <summary>
        /// Constructor
        /// </summary>
        public VisitApi(IVisitedService visitedService)
        {
            _visitedService = visitedService;
        }

        #region Insert Visit

        /// <summary>
        /// Insert New Visit
        /// </summary>
        public object Post(VisitRequest request)
        {
            try
            {
                var token = Request.Headers["token"];

                if (token == null)
                {
                    throw HttpError.Unauthorized("You are unauthorized to access this page");
                }

                _visitedService.InsertNewVisitedLocation(token, request.Location);
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
