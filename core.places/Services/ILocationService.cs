using core.places.dtos;
using System.Collections.Generic;
using System.IO;

namespace core.places.services
{
    public interface ILocationService
    {
        LocationDto ReturnLocationById(int locationId, bool isCity);
        List<LocationDto> ReturnLocationsForAutocomplete(string searchValue);
        List<LocationDto> ReturnLocationByParentId(int parentLocationId, string type);
        void UpdateLocation(LocationDto dto, bool isStaging);
        void UpdateLocation(LocationDto dto, bool isStaging, string reference);
		void AddLocation(LocationDto dto, bool isStaging);
		LocationDto AttachPhotos(string foreSquareId, LocationDto locationDto);
        PhotoList UploadPhoto(int locationId, Stream fileStream, string fileName, string contentType, string customerReference);
    }
}
