using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.flights.services;
using core.flights.dtos;

namespace triperoo.apis.endpoints.flights
{
    #region Return a list of cached flights

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/flights")]
    public class FlightCacheRequest
    {
        public string Market { get; set; }
        public string Currency { get; set; }
        public string Locale { get; set; }

        public string FlyFrom { get; set; }
        public string FlyTo { get; set; }
		public string DateFrom { get; set; }
		public string DateTo { get; set; }
        public string ReturnDate { get; set; }
        public string ReturnFrom { get; set; }
        public string ReturnTo { get; set; }
        public string FlightType { get; set; }
        public int PassengerTotal { get; set; }
        public int AdultTotal { get; set; }
        public int ChildTotal { get; set; }
        public int InfantTotal { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public string DepartureTimeFrom { get; set; }
        public string DepartureTimeTo { get; set; }
        public string ArrivalTimeFrom { get; set; }
        public string ArrivalTimeTo { get; set; }
        public string ReturnDepartureTimeFrom { get; set; }
        public string ReturnDepartureTimeTo { get; set; }
        public string ReturnArrivalTimeFrom { get; set; }
        public string ReturnArrivalTimeTo { get; set; }
        public string StopOverFrom { get; set; }
        public string StopOverTo { get; set; }
        public string Sort { get; set; }
        public string Asc { get; set; }
        public string SelectedAirlines { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int DirectFlightsOnly { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class FlightCacheRequestValidator : AbstractValidator<FlightCacheRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FlightCacheRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.FlyFrom).NotNull().WithMessage("Supply a valid outbound airport");
                RuleFor(r => r.FlyTo).NotNull().WithMessage("Supply a valid destination airport");
                RuleFor(r => r.Market).NotNull().WithMessage("Supply a valid market");
                RuleFor(r => r.Locale).NotNull().WithMessage("Supply a valid locale");
                RuleFor(r => r.Currency).NotNull().WithMessage("Supply a valid currency");
            });
        }
    }

    #endregion

    #region API logic

    public class FlightApi : Service
    {
        // Dependencies
        private readonly IFlightService _flightService;

        /// <summary>
        /// Constructor
        /// </summary>
        public FlightApi(IFlightService flightService)
        {
            _flightService = flightService;
        }

        /// <summary>
        /// List all cached flights
        /// </summary>
        public object Get(FlightCacheRequest request)
        {
            FlightListDto response = null;

            try
            {
                var dateFrom = request.DateFrom;
                var dateTo = request.DateTo;

                var df = DateTime.Parse(dateFrom);
                dateFrom = df.ToString("d");

                var dt = DateTime.Parse(dateTo);
                dateTo = dt.ToString("d");

                var returnFrom = request.ReturnFrom;
                var returnTo = request.ReturnTo;

                if (!string.IsNullOrEmpty(returnFrom))
                {
                    var rf = DateTime.Parse(returnFrom);
                    returnFrom = rf.ToString("d");
                }

                if (!string.IsNullOrEmpty(returnTo))
                {
                    var rt = DateTime.Parse(returnTo);
                    returnTo = rt.ToString("d");
                }

                response = _flightService.ReturnFlights(request.FlyFrom, request.FlyTo, dateFrom, dateTo, returnFrom, returnTo, request.FlightType, request.PassengerTotal, request.AdultTotal, request.ChildTotal, request.InfantTotal, request.PriceFrom, request.PriceTo, request.DepartureTimeFrom, request.DepartureTimeTo, request.ArrivalTimeFrom, request.ArrivalTimeTo, request.ReturnDepartureTimeFrom, request.ReturnDepartureTimeTo, request.ReturnArrivalTimeFrom, request.ReturnArrivalTimeTo, request.StopOverFrom, request.StopOverTo, request.Sort, request.Asc, request.SelectedAirlines, request.Offset, request.Limit, request.DirectFlightsOnly, request.Market, request.Currency, request.Locale);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }
    }

    #endregion
}
