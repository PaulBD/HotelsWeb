using core.places.dtos;

namespace core.places.services
{
    public interface IContentService
    {
        LocationListDto ReturnContentByParentRegionId(int parentLocationId);
        LocationListDto ReturnContentByParentRegionId(int parentLocationId, string contentType);
        LocationListDto ReturnContentByParentIdAndCategory(int parentLocationId, string contentType, string category);
    }
}
