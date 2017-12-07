using Funq;
using core.places.services;
using core.hotels.services;
using core.flights.services;
using core.customers.services;
using core.extras.services;
using library.weather.services;
using core.deals.Services;
using library.events.services;
using library.common;

namespace triperoo.apis.Configuration
{
    public class Services
    {
        public static void Register(Container container)
        {
            container.RegisterAutoWiredAs<EmailService, IEmailService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<EncryptionService, IEncryptionService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<LocationService, ILocationService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<ContentService, IContentService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<WeatherService, IWeatherService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<HotelService, IHotelService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<HotelPriceService, IHotelPriceService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<FlightService, IFlightService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<CustomerService, ICustomerService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<AuthorizeService, IAuthorizeService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<ActivityService, IActivityService>().ReusedWithin(ReuseScope.Container);
			container.RegisterAutoWiredAs<VisitedService, IVisitedService>().ReusedWithin(ReuseScope.Container);
			container.RegisterAutoWiredAs<FollowService, IFollowService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<LikeService, ILikeService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<PhotoService, IPhotoService>().ReusedWithin(ReuseScope.Container);
			container.RegisterAutoWiredAs<TripService, ITripService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<ReviewService, IReviewService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<QuestionService, IQuestionService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<ParkingService, IParkingService>().ReusedWithin(ReuseScope.Container);
			container.RegisterAutoWiredAs<AirportHotelService, IAirportHotelService>().ReusedWithin(ReuseScope.Container);
			container.RegisterAutoWiredAs<LoungeService, ILoungeService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<TravelzooService, ITravelzooService>().ReusedWithin(ReuseScope.Container);
            container.RegisterAutoWiredAs<EventService, IEventService>().ReusedWithin(ReuseScope.Container);
        }
    }
}