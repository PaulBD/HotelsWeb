using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using library.events.services;
using library.events.dtos;
using System.Linq;
using System.Collections.Generic;

namespace triperoo.apis.endpoints.locations
{
    #region Return events by location name

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/events", "GET")]
    public class EventRequest
    {
        public string Location { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

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
                RuleFor(r => r.Location).NotNull().WithMessage("Invalid location has been supplied");
                RuleFor(r => r.PageSize).NotNull().WithMessage("Invalid page size has been supplied");
                RuleFor(r => r.PageNumber).NotNull().WithMessage("Invalid page number has been supplied");
            });

        }
    }

    #endregion

    #region API logic

    public class EventApi : Service
    {
        private readonly IEventService _eventService;

        /// <summary>
        /// Constructor
        /// </summary>
        public EventApi(IEventService eventService)
        {
            _eventService = eventService;
        }

        #region List events by location

        /// <summary>
        /// Lists events by location
        /// </summary>
        public object Get(EventRequest request)
        {
            List<Event> response = new List<Event>();
            string cacheName = "event:" + request.Location + ":" + request.PageNumber;

            try
            {
                response = Cache.Get<List<Event>>(cacheName);

                if (response == null)
                {
                    var r = _eventService.ReturnEventsByLocation(request.Location, request.PageSize, request.PageNumber);

                    if (r != null)
                    {
                        response = r.events.Event.Where(q => q.image != null).ToList();
                        base.Cache.Add(cacheName, response);
                    }
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
