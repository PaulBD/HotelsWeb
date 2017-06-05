using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface IBookmarkService
    {
        void InsertNewBookmark(string token, int tripId, CustomerLocationDto bookmark);
        CustomerLocationDto ReturnBookmarkByLocationId(int locationId, int tripId, string token);
        void ArchiveBookmarkByLocationId(int locationId, int tripId, string token);
        List<CustomerLocationDto> ReturnBookmarksByToken(string token, int tripId);
    }
}
