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

		public ForesquarePhotosDto UpdatePhotos(string foresquareId)
		{
			if (!string.IsNullOrEmpty(foresquareId))
			{
				var url = "https://api.foursquare.com/v2/venues/" + foresquareId + "/photos?client_id=LR5CRGKSYR2L0DEL2RZMLYOYPUM5AFN3P0JXZO4IAGD5QZBV&client_secret=1N5TJBE35HP55X0NJCVEP15VTNMFMHAQJGVB2MC55KKAPWPX&v=20160608";

				var message = new HttpRequestMessage(HttpMethod.Get, url);

				var client = new HttpClient();
				var response = client.SendAsync(message).Result;

				if (response.IsSuccessStatusCode)
				{
					return JsonSerializer.DeserializeFromString<ForesquarePhotosDto>(response.Content.ReadAsStringAsync().Result);
				}
			}

			return null;
		}
    }
}
