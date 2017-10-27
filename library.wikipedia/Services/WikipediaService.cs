using System;
using System.Configuration;
using System.Net.Http;
using System.Text.RegularExpressions;
using library.wikipedia.dtos;
using ServiceStack.Text;

namespace library.wikipedia.services
{
    public class WikipediaService : IWikipediaService
    {
		public string ReturnContentByLocation(string venueName)
		{
			var url = string.Format(ConfigurationManager.AppSettings["wikipedia.url"], venueName);

			var message = new HttpRequestMessage(HttpMethod.Get, url);

			using (var client = new HttpClient())
			{
				var result = client.SendAsync(message).Result;

				if (result.IsSuccessStatusCode)
				{
					var response = JsonSerializer.DeserializeFromString<ContentDto>(result.Content.ReadAsStringAsync().Result.Replace("\"query\":{\"pages\":{\"", "\"query\":{\"pages\":{\"result_"));

                    if (response.query.pages.Count > 0)
                    {
                        var extract = response.query.pages.ToDictionary();

                        foreach(var s in extract)
                        {
                            var r = JsonSerializer.DeserializeFromString<PageDto>(s.Value);

                            if (r != null)
                            {
                                if (!string.IsNullOrEmpty(r.extract))
                                {
                                    var summary = Regex.Replace(r.extract, "<.*?>", String.Empty);
                                    return summary.Replace("\n", "<br /><br />");
                                }
                            }
                        }
                    }
                }
			}

			return null;
		}
    }
}
