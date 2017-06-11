using System;
namespace core.extras.Services
{
    public class BookingService : IBookingService
    {
        public BookingService()
        {
        }

        public void RetrieveBooking(string reference, string emailAddress)
        {
			/*
            var tokenResponse = GetUserToken();

            var url = ServerAddress + "/v1/booking/" + reference + ".js?key=" + Key + "&email=" + emailAddress;
           
            var message = new HttpRequestMessage(HttpMethod.Get, url);

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<AirportHotelResponseDto>(result.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
            */
		}
    }
}
