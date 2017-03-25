using library.events.dtos;

namespace library.events.services
{
    public interface IEventService
    {
        EventDto ReturnEventsByLocation(string location, int pageSize, int pageNumber);
    }
}
