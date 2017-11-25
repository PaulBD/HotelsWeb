using core.deals.dtos;
using System.Collections.Generic;

namespace core.deals.Services
{
    public interface ITravelzooService
    {
        List<TravelzooDto> ReturnDeals(string location);
        List<TravelzooDto> ReturnDealsExcludeLocation(string location);
        List<TravelzooDto> ReturnTopDeals();
    }
}
