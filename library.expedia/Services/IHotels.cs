using System;
using System.Collections.Generic;
using library.expedia.dtos;

namespace library.expedia.services
{
    public interface IHotels
    {
        HotelListDto ReturnHotelList(string sessionId, string locale, string currencyCode, string city, string countryCode, DateTime arrivalDate, int nights, List<string> rooms);
    }
}
