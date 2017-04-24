using core.places.dtos;
using System.Collections.Generic;

namespace core.places.services
{
    public interface IAttractionService
    {
        List<LocationDto> ReturnAttractionsByParentId(int parentLocationId);
        List<LocationDto> ReturnAttractionsByParentIdAndCategory(int parentLocationId, string category);
    }
}
