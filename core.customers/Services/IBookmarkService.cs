using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface IBookmarkService
    {
        void InsertNewBookmark(string token, BookmarkDto bookmark);
        BookmarkDto ReturnBookmarkByLocationId(int locationId, string token);
        void ArchiveBookmarkByLocationId(int locationId, string token);
        List<BookmarkDto> ReturnBookmarksByToken(string token);
    }
}
