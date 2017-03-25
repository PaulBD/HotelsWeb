using core.deals.dtos;
using System.Collections.Generic;

namespace core.deals.Services
{
    public interface ITravelzooService
    {
        List<TravelzooDto> ReturnDeals(string location, int limit, int offset);
    }
}
