using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface IBookmarkService
    {
        void InsertNewBookmark(string token, BookmarkDto bookmark);
        BookmarkDto ReturnBookmarkById(int id, string token);
        void ArchiveBookmarkById(int id, string token);
        List<BookmarkDto> ReturnBookmarksByToken(string token);
    }
}
