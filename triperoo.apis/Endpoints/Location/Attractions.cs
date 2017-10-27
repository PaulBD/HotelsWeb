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
    public class AttractionsRequest
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
    public class ParentAttractionValidator : AbstractValidator<AttractionsRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ParentAttractionValidator()
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
        public object Get(AttractionsRequest request)
        {
            AttractionListDto response = null;

            try
            {
                response = new AttractionListDto();

                response = _attractionService.ReturnAttractionsById(request.Id);
                response.LocationCount = response.Attractions.Count;

                if (!string.IsNullOrEmpty(request.CategoryName))
                {
                    response.Attractions = response.Attractions.Where(q => q.ProductCategories.ProductCategory.Any(e => e.Category.Trim().ToLower() == request.CategoryName.Trim().ToLower())).ToList();
                    //response.Attractions = response.Attractions.Where(q => q.ProductCategories.ProductCategory.FirstOrDefault(q => q.Category.Trim().ToLower() == request.CategoryName.Trim().ToLower())).ToList();
                    //response.MapLocations = response.MapLocations.Where(q => q.SubClass.ToLower() == request.CategoryName.ToLower()).ToList();
                }

                if (!string.IsNullOrEmpty(request.Name))
                {
                    var list = response.Attractions.Where(q => q.ProductName.en.ToLower().Contains(request.Name.ToLower())).ToList();
                    response.Attractions = list;
                    response.LocationCount = list.Count;
                }

                if (response.LocationCount > request.PageSize)
                {
                    response.Attractions = response.Attractions.Skip(request.PageSize * request.PageNumber).Take(request.PageSize).ToList();
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
