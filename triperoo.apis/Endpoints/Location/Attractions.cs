using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;
using System.Linq;

namespace triperoo.apis.endpoints.locations
{
    #region Return Attractions by Location Id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/location/{id}/attractions", "GET")]
    public class ParentAttractionRequest
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
    public class ParentAttractionRequestValidator : AbstractValidator<ParentAttractionRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ParentAttractionRequestValidator()
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

    public class AttractionsApi : Service
    {
        private readonly IAttractionService _attractionService;

        /// <summary>
        /// Constructor
        /// </summary>
        public AttractionsApi(IAttractionService attractionService)
        {
            _attractionService = attractionService;
        }

		#region Return Attractions by Location Id

		/// <summary>
		/// Return Attractions by Location Id
		/// </summary>
		public object Get(ParentAttractionRequest request)
        {
            string cacheName = "attractions:" + request.Id + request.CategoryName;
            LocationListDto response = null;

            try
            {
                response = Cache.Get<LocationListDto>(cacheName);

                if (response == null)
                {
                    response = new LocationListDto();

                    if (!string.IsNullOrEmpty(request.CategoryName))
                    {
                        response.Locations = _attractionService.ReturnAttractionsByParentIdAndCategory(request.Id, request.CategoryName);
                    }
                    else
                    {
                        response.Locations = _attractionService.ReturnAttractionsByParentId(request.Id);
                    }

                    response.LocationCount = response.Locations.Count;
                   // Cache.Add(cacheName, response, new DateTime().AddHours(24));
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
