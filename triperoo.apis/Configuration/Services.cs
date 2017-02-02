using Funq;
using core.places.services;
using core.hotels.services;
using core.flights.services;
using core.customers.services;

namespace triperoo.apis.Configuration
{
    public class Services
    {
        public static void Register(Container container)
        {
            container.RegisterAutoWiredAs<LocationService, ILocationService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<PlaceService, IPlaceService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<HotelService, IHotelService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<HotelPriceService, IHotelPriceService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<FlightService, IFlightService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<CustomerService, ICustomerService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<AuthorizeService, IAuthorizeService>().ReusedWithin(ReuseScope.Container);
        }
    }
}