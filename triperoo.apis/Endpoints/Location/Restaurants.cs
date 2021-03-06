﻿using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;
using System.Linq;

namespace triperoo.apis.endpoints.location
{
	#region List Restaurants By Location Id

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/location/{id}/restaurants", "GET")]
    public class ParentRestaurantRequest
    {
        public int Id { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string CategoryName { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class ParentRestaurantRequestValidator : AbstractValidator<ParentRestaurantRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ParentRestaurantRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.Id).GreaterThan(0).WithMessage("Invalid location id have been supplied");
                RuleFor(r => r.PageSize).GreaterThan(0).WithMessage("Invalid page size has been supplied");
                RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(0).WithMessage("Invalid page number has been supplied");
            });
        }
    }

    #endregion

    #region API logic

    public class RestaurantsApi : Service
    {
        private readonly IRestaurantService _restaurantService;

        /// <summary>
        /// Constructor
        /// </summary>
        public RestaurantsApi(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

		#region List Restaurants By Location Id

		/// <summary>
		/// Lists restaurants by location Id
		/// </summary>
		public object Get(ParentRestaurantRequest request)
        {
            LocationListDto response = null;

            try
            {
                response = new LocationListDto();

                response = _restaurantService.ReturnRestaurantsByParentId(request.Id);
                response.LocationCount = response.Locations.Count;

                if (!string.IsNullOrEmpty(request.CategoryName))
                {
                    response.Locations = response.Locations.Where(q => q.SubClass.ToLower() == request.CategoryName.ToLower()).ToList();
                    response.MapLocations = response.MapLocations.Where(q => q.SubClass.ToLower() == request.CategoryName.ToLower()).ToList();
                }

                if (response.LocationCount > request.PageSize)
                {
                    response.Locations = response.Locations.Skip(request.PageSize * request.PageNumber).Take(request.PageSize).ToList();
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
