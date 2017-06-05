using library.events.dtos;
using System.Net.Http;
using System.Configuration;
using ServiceStack.Text;

namespace library.events.services
{
    public class EventService : IEventService
    {
        /// <summary>
        /// Return events by location (i.e London)
        /// </summary>
        public EventDto ReturnEventsByLocation(string location, string category, int pageSize, int pageNumber)
        {
            var url = string.Format(ConfigurationManager.AppSettings["events.eventful.url"], location, pageSize, pageNumber);

            if (!string.IsNullOrEmpty(category))
            {
                url += "&category=" + category + "&date=Future";
            }
            else {
                url += "&within=30&date=Future";
            }

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<EventDto>(result.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
		}

        /// <summary>
        /// Return events by location name (i.e Kensington Palace)
        /// </summary>
		public EventDto ReturnEventsByLocationName(string locationName, int pageSize, int pageNumber)
		{
			var url = string.Format(ConfigurationManager.AppSettings["location.eventful.url"], locationName, pageSize, pageNumber);

			var message = new HttpRequestMessage(HttpMethod.Get, url);

			using (var client = new HttpClient())
			{
				var result = client.SendAsync(message).Result;

				if (result.IsSuccessStatusCode)
				{
					return JsonSerializer.DeserializeFromString<EventDto>(result.Content.ReadAsStringAsync().Result);
				}
			}

			return null;
		}
    }
}
