using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;
using System.Linq;

namespace triperoo.apis.endpoints.location
{
    #region List Nightlife By Location Id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/location/{id}/nightlife", "GET")]
    public class ParentNightlifeRequest
    {
        public int Id { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string CategoryName { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class ParentNightlifeRequestValidator : AbstractValidator<ParentNightlifeRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ParentNightlifeRequestValidator()
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

    public class NightlifeApi : Service
    {
        private readonly INightlifeService _nightlifeService;

        /// <summary>
        /// Constructor
        /// </summary>
        public NightlifeApi(INightlifeService nightlifeService)
        {
            _nightlifeService = nightlifeService;
        }

		#region List Nightlife By Location Id

		/// <summary>
		/// List Nightlife By Location Id
		/// </summary>
		public object Get(ParentNightlifeRequest request)
        {
            string cacheName = "nightlife:" + request.Id + request.CategoryName;
            LocationListDto response = null;

            try
            {
                response = Cache.Get<LocationListDto>(cacheName);

                if (response == null)
                {
                    response = new LocationListDto();

                    if (!string.IsNullOrEmpty(request.CategoryName))
                    {
                        response.Locations = _nightlifeService.ReturnNightlifeByParentIdAndCategory(request.Id, request.CategoryName);
                    }
                    else
                    {
                        response.Locations = _nightlifeService.ReturnNightlifeByParentId(request.Id);
					}

					response.LocationCount = response.Locations.Count;
                    //base.Cache.Add(cacheName, response);
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
