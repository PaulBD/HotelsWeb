using library.foursquare.dtos;
using System.Net.Http;
using System.Configuration;
using ServiceStack.Text;

namespace library.foursquare.services
{
    public class VenueService : IVenueService
    {
        public VenueDto ReturnVenuesByLocation(string venueName, string location)
        {
            var url = string.Format(ConfigurationManager.AppSettings["foursquare.url"]
                                    , ConfigurationManager.AppSettings["foursquare.clientId"]
                                    , ConfigurationManager.AppSettings["foursquare.clientSecret"]
                                    , venueName, location);

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<VenueDto>(result.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
        }
    }
}
