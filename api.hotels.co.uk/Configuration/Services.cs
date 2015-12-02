using Funq;
using api.laterooms.co.uk.Services;

namespace hotels.triperoo.co.uk.Configuration
{
    public class Services
    {
        public static void Register(Container container)
        {
            container.RegisterAutoWiredAs<LateRoomsApiService, ILateRoomsApiService>().ReusedWithin(ReuseScope.Container);
        }
    }
}