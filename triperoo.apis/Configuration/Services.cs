using Funq;
using core.places.services;
using core.hotels.services;
using core.flights.services;
using core.customers.services;
using core.extras.services;
using library.weather.services;
using core.deals.Services;
using library.events.services;

namespace triperoo.apis.Configuration
{
    public class Services
    {
        public static void Register(Container container)
        {
            container.RegisterAutoWiredAs<LocationService, ILocationService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<WeatherService, IWeatherService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<HotelService, IHotelService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<HotelPriceService, IHotelPriceService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<FlightService, IFlightService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<CustomerService, ICustomerService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<AuthorizeService, IAuthorizeService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<FavouriteService, IFavouriteService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<ReviewService, IReviewService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<QuestionService, IQuestionService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<ParkingService, IParkingService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<TravelzooService, ITravelzooService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<EventService, IEventService>().ReusedWithin(ReuseScope.Container);
        }
    }
}