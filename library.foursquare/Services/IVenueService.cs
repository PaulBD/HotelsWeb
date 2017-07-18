using library.foursquare.dtos;

namespace library.foursquare.services
{
    public interface IVenueService
    {
		VenueDto ReturnVenuesByLocation(string venueName, string location);
        ForesquarePhotosDto UpdatePhotos(string foresquareId);
    }
}
