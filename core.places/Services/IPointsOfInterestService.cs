using core.places.dtos;

namespace core.places.services
{
    public interface IPointOfInterestService
    {
        LocationListDto ReturnPointOfInterestsByParentId(int parentLocationId);
        LocationListDto ReturnPointOfInterestsByParentIdAndCategory(int parentLocationId, string category);
    }
}
