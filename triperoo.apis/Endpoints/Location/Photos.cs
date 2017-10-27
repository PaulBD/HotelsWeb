using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;
using System.Collections.Generic;
using core.customers.services;

namespace triperoo.apis.endpoints.location
{
	#region Upload Photos 

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/location/{id}/photos", "POST")]
	public class LocationPhotosRequest
	{
		public int id { get; set; }

	}

	/// <summary>
	/// Validator
	/// </summary>
	public class LocationPhotosRequestValidator : AbstractValidator<LocationPhotosRequest>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public LocationPhotosRequestValidator()
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

    public class LocationPhotosApi : Service
    {
		private readonly ILocationService _locationService;
        private readonly ICustomerService _customerService;
        private readonly IPhotoService _photoService;

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationPhotosApi(ILocationService locationService, ICustomerService customerService, IPhotoService photoService)
        {
			_locationService = locationService;
            _customerService = customerService;
            _photoService = photoService;
        }

		#region Upload Photos

		/// <summary>
		/// Upload Photos
		/// </summary>
		public object Post(LocationPhotosRequest request)
        {
            try
			{
				var token = Request.Headers["token"];

				if (string.IsNullOrEmpty(token))
				{
					return new HttpResult("Token not found", HttpStatusCode.Unauthorized);
				}

				var customer = _customerService.ReturnCustomerByToken(token);

                if (customer == null)
                {
                    return new HttpResult("Customer not found" + token, HttpStatusCode.Unauthorized);
                }

				var files = Request.Files;

                foreach (var f in files)
                {
                    var photo = _locationService.UploadPhoto(request.id, f.InputStream, f.FileName, f.ContentType, customer.TriperooCustomers.CustomerReference);

                    _photoService.InsertNewPhoto(request.id, photo, customer.TriperooCustomers.CustomerReference);
                }
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
