using System.Configuration;
using System.Net.Http;
using library.zomato.dtos;
using ServiceStack.Text;

namespace library.zomato.services
{
    public class RestaurantService : IRestaurantService
    {
        public RestaurantDto ReturnRestaurantByLocation(double latitude, double longitude)
        {
            var url = string.Format(ConfigurationManager.AppSettings["zomato.url"], latitude, longitude);

            var message = new HttpRequestMessage(HttpMethod.Get, url);
            message.Headers.Add("user-key", ConfigurationManager.AppSettings["zomato.key"]);

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<RestaurantDto>(result.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
        }
    }
}
