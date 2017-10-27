using core.places.dtos;

namespace core.places.services
{
    public interface IAttractionService
    {
        AttractionListDto ReturnAttractionsById(int id);
    }
}
