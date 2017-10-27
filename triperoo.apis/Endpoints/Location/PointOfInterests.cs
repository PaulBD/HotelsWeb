using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;
using System.Linq;

namespace triperoo.apis.endpoints.locations
{
    #region Return Point Of Interest by Location Id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/location/{id}/pointofinterest", "GET")]
    public class ParentPointOfInterestRequest
    {
        public int Id { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
		public string CategoryName { get; set; }
		public string Name { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class ParentPointOfInterestRequestValidator : AbstractValidator<ParentPointOfInterestRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ParentPointOfInterestRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.Id).GreaterThan(0).WithMessage("Invalid location id has been supplied");
                RuleFor(r => r.PageSize).GreaterThan(0).WithMessage("Invalid page size has been supplied");
                RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(0).WithMessage("Invalid page number has been supplied");
            });
        }
    }

    #endregion

    #region API logic

    public class PointOfInterestApi : Service
    {
        private readonly IPointOfInterestService _pointOfInterestService;

        /// <summary>
        /// Constructor
        /// </summary>
        public PointOfInterestApi(IPointOfInterestService pointOfInterestService)
        {
            _pointOfInterestService = pointOfInterestService;
        }

		#region Return Attractions by Location Id

		/// <summary>
		/// Return Attractions by Location Id
		/// </summary>
        public object Get(ParentPointOfInterestRequest request)
        {

            LocationListDto response = null;

            try
            {
                response = new LocationListDto();

                response = _pointOfInterestService.ReturnPointOfInterestsByParentId(request.Id);
				response.LocationCount = response.Locations.Count;

				if (!string.IsNullOrEmpty(request.CategoryName))
				{
					response.Locations = response.Locations.Where(q => q.SubClass.ToLower() == request.CategoryName.ToLower()).ToList();
					response.MapLocations = response.MapLocations.Where(q => q.SubClass.ToLower() == request.CategoryName.ToLower()).ToList();
				}

                if (!string.IsNullOrEmpty(request.Name))
                {
                    var list = response.Locations.Where(q => q.RegionName.ToLower().Contains(request.Name.ToLower())).ToList();
                    response.Locations = list;
                    response.LocationCount = list.Count;
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
