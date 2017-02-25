using System.Configuration;
using System.Net.Http;

namespace core.extras.services
{
    public class BaseService
    {
        protected HttpClient Client { get; set; }
        protected string ServerAddress { get; set; }
        protected const string Key = "billington";
        
        public BaseService()
        {
            ServerAddress = "http://api.holidayextras.co.uk";

            if (ConfigurationManager.AppSettings["Environment"] == "Dev")
            {
                ServerAddress = "http://api.holidayextras.co.uk/sandbox";
            }

            Client = new HttpClient();
        }
    }
}
