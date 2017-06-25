using library.wikipedia.dtos;

namespace library.wikipedia.services
{
    public interface IContentService
    {
        string ReturnContentByLocation(string venueName);
    }
}
