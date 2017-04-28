using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using library.events.services;
using library.events.dtos;
using System.Linq;
using core.places.services;
using core.places.dtos;

namespace triperoo.apis.endpoints.locations
{
    #region Return events by location name

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/location/{id}/events", "GET")]
    public class EventRequest
    {
        public int Id { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string CategoryName { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class EventRequestValidator : AbstractValidator<EventRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EventRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.Id).GreaterThan(0).WithMessage("Invalid location id has been supplied");
                RuleFor(r => r.PageSize).GreaterThan(0).WithMessage("Invalid page size has been supplied");
                RuleFor(r => r.PageNumber).GreaterThan(0).WithMessage("Invalid page number has been supplied");
            });

        }
    }

    #endregion

    #region API logic

    public class EventApi : Service
    {
        private readonly IEventService _eventService;
        private readonly ILocationService _locationService;

        /// <summary>
        /// Constructor
        /// </summary>
        public EventApi(IEventService eventService, ILocationService locationService)
        {
            _locationService = locationService;
            _eventService = eventService;
        }

        #region List events by location

        /// <summary>
        /// Lists events by location
        /// </summary>
        public object Get(EventRequest request)
        {
            EventDto eventResponse = new EventDto();
            LocationDto locationResponse = new LocationDto();
            string cacheName = "event:" + request.Id + ":" + request.CategoryName;
            string locationCacheName = "location:" + request.Id;

            try
            {
                locationResponse = Cache.Get<LocationDto>(locationCacheName);

                if (locationResponse == null)
                {
                    locationResponse = _locationService.ReturnLocationById(request.Id);
                    base.Cache.Add(locationCacheName, locationResponse);
                }

                eventResponse = Cache.Get<EventDto>(cacheName);

                if (eventResponse == null)
                {
                    if (request.CategoryName == "all")
                    {
                        eventResponse = _eventService.ReturnEventsByLocation(locationResponse.RegionName, null, 10, request.PageNumber + 1);
                    }
                    else
                    {
                        eventResponse = _eventService.ReturnEventsByLocation(locationResponse.RegionName, request.CategoryName, 10, request.PageNumber + 1);
                    }

                    // base.Cache.Add(cacheName, response);
                }

                if (eventResponse.events != null)
                {
                    eventResponse.events.Event = eventResponse.events.Event.Take(request.PageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(eventResponse, HttpStatusCode.OK);
        }

        #endregion
    }

    #endregion
}
