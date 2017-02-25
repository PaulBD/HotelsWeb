using core.extras.dtos;
using ServiceStack.Text;
using System.Net.Http;

namespace core.extras.services
{
    public class UserTokenService : BaseService, IUserTokenService
    {
        /// <summary>
        /// Get User Token
        /// </summary>
        public UserTokenDto GetUserToken()
        {
            var url = ServerAddress + "/v1/usertoken.js?key=" + Key;
            
            var message = new HttpRequestMessage(HttpMethod.Get, url);

            var client = new HttpClient();
            var result = client.SendAsync(message).Result;

            if (result.IsSuccessStatusCode)
            {
                return JsonSerializer.DeserializeFromString<UserTokenDto>(result.Content.ReadAsStringAsync().Result);
            }

            return null;
        }
    }
}
