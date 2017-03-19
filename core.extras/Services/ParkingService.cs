using System;
using core.extras.dtos;
using System.Net.Http;
using ServiceStack.Text;

namespace core.extras.services
{
    public class ParkingService : BaseService, IParkingService
    {
        #region Parking Availability

        /// <summary>
        /// Parking Availability
        /// </summary>
        public ParkingAvailabilityResponseDto AvailabilityAtDestination(ParkingAvailabilityRequestDto request)
        {
            var tokenResponse = GetUserToken();

            var url = ServerAddress + "/v1/carpark/" + request.Location.LocationName + ".js?key=" + Key;
            url += "&ArrivalDate=" + String.Format("{0:yyyy-MM-dd}", request.Location.ArrivalDate);
            url += "&ArrivalTime=" + String.Format("{0:HHmm}", request.Location.ArrivalDate);
            url += "&DepartDate=" + String.Format("{0:yyyy-MM-dd}", request.Location.DepartDate);
            url += "&DepartTime=" + String.Format("{0:HHmm}", request.Location.DepartDate);
            url += "&token=" + tokenResponse.API_Reply.Token;
            
            if (!string.IsNullOrEmpty(request.Lang))
            {
                url += "&Lang=" + request.Lang;
            }

            if (request.Passenger.PassengerCount > 0)
            {
                url += "&NumberOfPax=" + request.Passenger.PassengerCount;
            }

            if (!string.IsNullOrEmpty(request.Passenger.Initials))
            {
                url += "&Initials=" + request.Passenger.Initials;
            }

            if (request.Location.IsAirport)
            {
                if (!request.Location.IsUnitedKingdom)
                {
                    url += "&System=ABG";
                }

                if (!string.IsNullOrEmpty(request.Fields))
                {
                    url += "&Fields=" + request.Fields;
                }

                if (!string.IsNullOrEmpty(request.Filter))
                {
                    url += "&Filter=" + request.Filter;
                }
            }

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<ParkingAvailabilityResponseDto>(result.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
        }

        #endregion

        #region Ignore

        #region Return Space Count

        /// <summary>
        /// Return Space Count
        /// </summary>
        public ParkingCountDto ReturnSpaceCount(string destination, DateTime arrivalDate, DateTime departDate, string initials, string userToken)
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

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<ParkingCountDto>(result.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
        }

        #endregion

        #region Upgrade Parking

        /// <summary>
        /// Upgrade Parking
        /// </summary>
        public ParkingUpgradesDto ParkingUpgrade(string productCode, DateTime arrivalDate, DateTime departDate, int adultCount, string[] roomCodes, string userToken)
        {
            var url = ServerAddress + "/v1/upgrade/" + productCode + ".js?key=" + Key;
            url += "&ArrivalDate=" + String.Format("{0:yyyy-MM-dd}", arrivalDate);
            url += "&DepartDate=" + String.Format("{0:yyyy-MM-dd}", departDate);
            url += "&AdultCount=" + adultCount;
            url += "&RoomCode=" + roomCodes;
            url += "&token=" + userToken;

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<ParkingUpgradesDto>(result.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
        }

        #endregion

        #region Book Car Park

        public void BookCarPark(ParkingBookingRequestDto parkingDto)
        {
            /*var url = ServerAddress + "/v1/carpark/" + productCode + ".js?key=" + Key;

            var message = new HttpRequestMessage(HttpMethod.Post, url);

            using (var client = new HttpClient())
            {
            }
            */
        }

        #endregion

        #endregion
    }
}
