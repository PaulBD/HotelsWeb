using core.places.dtos;

namespace core.places.services
{
    public interface IRestaurantService
    {
        LocationListDto ReturnRestaurantsByParentId(int parentLocationId);
        LocationListDto ReturnRestaurantsByParentIdAndCategory(int parentLocationId, string category);
    }
}
