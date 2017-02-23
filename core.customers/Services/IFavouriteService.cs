using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface IFavouriteService
    {
        void InsertNewFavourite(string token, FavouriteDto favourite);
        FavouriteDto ReturnFavouriteById(int id, string token);
        void ArchiveFavouriteById(int id, string token);
        List<FavouriteDto> ReturnFavouritesByToken(string token);
        List<FavouriteDto> ReturnFavouritesByPlaceReference(string reference);
    }
}
