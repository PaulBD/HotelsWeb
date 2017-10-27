using core.places.dtos;

namespace core.places.services
{
    public interface INightlifeService
    {
        LocationListDto ReturnNightlifeByParentId(int parentLocationId);
        LocationListDto ReturnNightlifeByParentIdAndCategory(int parentLocationId, string category);
    }
}
