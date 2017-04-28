using core.places.dtos;
using System.Collections.Generic;

namespace core.places.services
{
    public interface INightlifeService
    {
        List<LocationDto> ReturnNightlifeByParentId(int parentLocationId);
        List<LocationDto> ReturnNightlifeByParentIdAndCategory(int parentLocationId, string category);
    }
}
