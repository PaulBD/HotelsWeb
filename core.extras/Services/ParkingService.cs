using System;
using core.extras.dtos;
using System.Net.Http;
using ServiceStack.Text;

namespace core.extras.services
{
    public class ParkingService : BaseService, IParkingService
    {
        public ParkingDto AvailabilityAtDestination(string destination, bool isAirport, DateTime arrivalDate, DateTime departDate, bool isUnitedKingdom, string userToken, string initials, int passengerCount, string filter, string fields, string lang)
        {
            var url = ServerAddress + "/v1/carpark/" + destination + ".js?key=" + Key;
            url += "&ArrivalDate=" + String.Format("{0:yyyy-MM-dd}", arrivalDate);
            url += "&ArrivalTime=" + String.Format("{0:HHmm}", arrivalDate);
            url += "&DepartDate=" + String.Format("{0:yyyy-MM-dd}", departDate);
            url += "&DepartTime=" + String.Format("{0:HHmm}", departDate);
            url += "&token=" + userToken;
            
            if (!string.IsNullOrEmpty(lang))
            {
                url += "&Lang=" + lang;
            }

            if (passengerCount > 0)
            {
                url += "&NumberOfPax=" + passengerCount;
            }

            if (!string.IsNullOrEmpty(initials))
            {
                url += "&Initials=" + initials;
            }

            if (isAirport)
            {
                if (!isUnitedKingdom)
                {
                    url += "&System=ABG";
                }

                if (!string.IsNullOrEmpty(fields))
                {
                    url += "&Fields=" + fields;
                }

                if (!string.IsNullOrEmpty(filter))
                {
                    url += "&Filter=" + filter;
                }
            }

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            var client = new HttpClient();
            var result = client.SendAsync(message).Result;

            if (result.IsSuccessStatusCode)
            {
                return JsonSerializer.DeserializeFromString<ParkingDto>(result.Content.ReadAsStringAsync().Result);
            }

            return null;
        }

        public ParkingCountDto CheckSpaceCount(string destination, DateTime arrivalDate, DateTime departDate, string initials, string userToken)
        {
            var url = ServerAddress + "/v1/carparkspaces/" + destination + ".js?key=" + Key;
            url += "&ArrivalDate=" + String.Format("{0:yyyy-MM-dd}", arrivalDate);
            url += "&ArrivalTime=" + String.Format("{0:HHmm}", arrivalDate);
            url += "&DepartDate=" + String.Format("{0:yyyy-MM-dd}", departDate);
            url += "&DepartTime=" + String.Format("{0:HHmm}", departDate);
            url += "&token=" + userToken;

            if (!string.IsNullOrEmpty(initials))
            {
                url += "&Initials=" + initials;
            }

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            var client = new HttpClient();
            var result = client.SendAsync(message).Result;

            if (result.IsSuccessStatusCode)
            {
                return JsonSerializer.DeserializeFromString<ParkingCountDto>(result.Content.ReadAsStringAsync().Result);
            }

            return null;
        }

        public ParkingUpgradesDto ParkingUpgrade(string productCode, DateTime arrivalDate, DateTime departDate, int adultCount, string[] roomCodes, string userToken)
        {
            var url = ServerAddress + "/v1/upgrade/" + productCode + ".js?key=" + Key;
            url += "&ArrivalDate=" + String.Format("{0:yyyy-MM-dd}", arrivalDate);
            url += "&DepartDate=" + String.Format("{0:yyyy-MM-dd}", departDate);
            url += "&AdultCount=" + adultCount;
            url += "&RoomCode=" + roomCodes;
            url += "&token=" + userToken;

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            var client = new HttpClient();
            var result = client.SendAsync(message).Result;

            if (result.IsSuccessStatusCode)
            {
                return JsonSerializer.DeserializeFromString<ParkingUpgradesDto>(result.Content.ReadAsStringAsync().Result);
            }

            return null;
        }
    }
}
