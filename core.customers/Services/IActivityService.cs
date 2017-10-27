using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface IActivityService
    {
        void InsertNewActivity(string token, int tripId, ActivityDto activity);
        ActivityDto ReturnActivityByLocationId(int locationId, int tripId, string token);
        void ArchiveActivityByLocationId(int locationId, int tripId, string token);
        List<ActivityDto> ReturnActivitiesByToken(string token, int tripId);
    }
}
