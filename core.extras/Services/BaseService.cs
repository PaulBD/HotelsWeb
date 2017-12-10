using core.extras.dtos;
using ServiceStack.Text;
using System.Configuration;
using System.Net.Http;

namespace core.extras.services
{
    public class BaseService
    {
        protected string ServerAddress { get; set; }
        protected const string Key = "billington";
        
        public BaseService()
        {
            ServerAddress = "https://api.holidayextras.co.uk";
/*
            if (ConfigurationManager.AppSettings["Environment"] == "Dev")
            {
                ServerAddress = "https://api.holidayextras.co.uk/sandbox";
            }
            */
        }

        /// <summary>
        /// Get User Token
        /// </summary>
        public UserTokenDto GetUserToken()
        {
            var url = ServerAddress + "/v1/usertoken.js?key=" + Key;

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<UserTokenDto>(result.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
        }
    }
}
