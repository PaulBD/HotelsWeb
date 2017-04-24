using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;
using System.Linq;

namespace triperoo.apis.endpoints.locations
{
    #region Return attractions by parent id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/attractions", "GET")]
    public class ParentAttractionRequest
    {
        public int ParentLocationId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string CategoryName { get; set; }
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
                RuleFor(r => r.ParentLocationId).GreaterThan(0).WithMessage("Invalid parent location id have been supplied");
                RuleFor(r => r.PageSize).NotNull().WithMessage("Invalid page size has been supplied");
                RuleFor(r => r.PageNumber).NotNull().WithMessage("Invalid page number has been supplied");
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

        #region List Attractions by Parent Id

        /// <summary>
        /// Lists attractions by parent Id
        /// </summary>
        public object Get(ParentAttractionRequest request)
        {
            string cacheName = "Attractions:" + request.ParentLocationId + request.CategoryName;
            LocationListDto response = null;

            try
            {
                response = Cache.Get<LocationListDto>(cacheName);

                if (response == null)
                {
                    response = new LocationListDto();

                    if (!string.IsNullOrEmpty(request.CategoryName))
                    {
                        response.Locations = _attractionService.ReturnAttractionsByParentIdAndCategory(request.ParentLocationId, request.CategoryName);
                    }
                    else
                    {
                        response.Locations = _attractionService.ReturnAttractionsByParentId(request.ParentLocationId);
                    }
                    response.LocationCount = response.Locations.Count;
                    //base.Cache.Add(cacheName, response);
                }

                response.Locations = response.Locations.Skip(request.PageSize * request.PageNumber).Take(request.PageSize).ToList(); 
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
