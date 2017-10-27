using library.wikipedia.dtos;

namespace library.wikipedia.services
{
    public interface IWikipediaService
    {
        string ReturnContentByLocation(string venueName);
    }
}
