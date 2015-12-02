using System.Collections.Generic;
using dtos.laterooms.co.uk;
using api.laterooms.co.uk.Enums;

namespace api.laterooms.co.uk.Services
{
    public interface ILateRoomsApiService
    {
        /// <summary>
        /// Return selected Hotels
        /// </summary>
        LateRoomsDto SearchSelectedHotels(List<int> hids, LateroomsCurrency currency, string startDate, int nights);

        /// <summary>
        /// Return Hotels By Keyword
        /// </summary>
        LateRoomsDto SearchHotelsByKeyword(string keyword, LateroomsCurrency currency, string startDate, int nights);

        /// <summary>
        /// Return Hotel Live Prices
        /// </summary>
        LivePricesDto SearchLivePrices(string hid, LateroomsCurrency currency, string startDate, int nights);
    }
}
