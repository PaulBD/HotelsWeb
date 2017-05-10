using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;

namespace triperoo.apis.endpoints.location
{
	#region Return Single Location By Id

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/location/{id}", "GET")]
    public class LocationRequest
    {
        public int id { get; set; }

    }

    /// <summary>
    /// Validator
    /// </summary>
    public class LocationRequestValidator : AbstractValidator<LocationRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LocationRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.id).GreaterThan(0).WithMessage("Invalid location id has been supplied");
            });

        }
    }

    #endregion

    #region API logic

    public class LocationApi : Service
    {
        private readonly ILocationService _locationService;

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationApi(ILocationService locationService)
        {
            _locationService = locationService;
        }

		#region Return Single Location By Id

		/// <summary>
		/// Return Single Location By Id
		/// </summary>
		public object Get(LocationRequest request)
        {
            LocationDto response = null;
            string cacheName = "location:" + request.id;

            try
            {
                response = Cache.Get<LocationDto>(cacheName);

                if (response == null)
                {
                    response = _locationService.ReturnLocationById(request.id);
                    base.Cache.Add(cacheName, response);
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        #endregion
    }

    #endregion
}
