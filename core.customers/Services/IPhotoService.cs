using core.customers.dtos;

namespace core.customers.services
{
    public interface IPhotoService
    {
        CustomerPhotos ReturnPhotosByReference(string reference);
        void ArchivePhotoById(string token, string photoReference);
        void InsertNewPhoto(int regionID, places.dtos.PhotoList photo, string reference);
    }
}
