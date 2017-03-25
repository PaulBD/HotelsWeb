using library.weather.dtos;
using System.Net.Http;
using System.Configuration;
using ServiceStack.Text;

namespace library.weather.services
{
    public class WeatherService : IWeatherService
    {
        /// <summary>
        /// Return Weather By latitude & Longitude
        /// </summary>
        public WeatherDto ReturnWeatherByLocation(double latitude, double longitude, string language = "en")
        {
            var url = string.Format(ConfigurationManager.AppSettings["weather.darksky.url"], latitude, longitude, language);

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<WeatherDto>(result.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
        }
    }
}
