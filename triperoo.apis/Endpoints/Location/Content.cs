using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;
using System.Linq;
using System.Collections.Generic;

namespace triperoo.apis.endpoints.locations
{
    #region Return Content by Location Id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/location/{id}/{type}", "GET")]
    public class ParentContentRequest
    {
        public int Id { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public List<string> CategoryName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class ParentContentRequestValidator : AbstractValidator<ParentContentRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ParentContentRequestValidator()
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

    public class ContentApi : Service
    {
        private readonly IContentService _contentService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ContentApi(IContentService contentService)
        {
            _contentService = contentService;
        }

        #region Return Content by Location Id

        /// <summary>
        /// Return Content by Location Id
        /// </summary>
        public object Get(ParentContentRequest request)
        {
            LocationListDto response = null;

            try
            {
                response = new LocationListDto();

                var requestType = request.Type;

                switch (requestType)
                {
                    case "Regions":
                    case "Multi-Regions":
                        response = _contentService.ReturnContentByParentRegionId(request.Id);
                        break;
                    default:
                        response = _contentService.ReturnContentByParentRegionId(request.Id, requestType);
                        break;
                }

                response.LocationCount = response.Locations.Count;

                if (request.CategoryName != null)
                {
                    if (request.CategoryName.Count > 0)
                    {
                        response.Locations = response.Locations.Where(x => request.CategoryName.Contains(x.SubClass.Replace("0", ",").Replace(" & ", "-and-").Replace(" ", "-").ToLower())).ToList();
                        response.MapLocations = response.MapLocations.Where(x => request.CategoryName.Contains(x.SubClass.Replace("0", ",").Replace(" & ", "-and-").Replace(" ", "-").ToLower())).ToList();
                    }
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
