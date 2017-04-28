using core.places.dtos;
using System.Collections.Generic;

namespace core.places.services
{
    public interface IRestaurantService
    {
        List<LocationDto> ReturnRestaurantsByParentId(int parentLocationId);
        List<LocationDto> ReturnRestaurantsByParentIdAndCategory(int parentLocationId, string category);
    }
}
