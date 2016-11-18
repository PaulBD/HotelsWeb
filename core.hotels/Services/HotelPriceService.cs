using core.hotels.dtos;
using core.hotels.enums;
using core.hotels.utility;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Xml;

namespace core.hotels.services
{
    public class HotelPriceService : IHotelPriceService
    {
        private readonly HttpClient _client;
        private readonly string _affiliateId;
        private readonly Common _common;

        public HotelPriceService()
        {
            _common = new Common();
            _affiliateId = ConfigurationManager.AppSettings["hotels.laterooms.affiliateId"];

            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["hotels.laterooms.baseUrl"])
            };
        }

        /// <summary>
        /// Return hotel price using id
        /// </summary>
        public HotelPriceDto ReturnHotelPrice(string id, Currency currency, string startDate, int nights)
        {
            var request = "index.aspx?aid=" + _affiliateId + "&rtype=7&hids=" + id + "&cur=" + _common.ReturnCurrency(currency) + "&sdate=" + startDate + "&nights=" + nights;
            
            var doc = new XmlDocument();

            var response = _client.GetAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                result = _common.NormaliseResponse(result);

                doc.LoadXml(result);
                result = JsonConvert.SerializeXmlNode(doc);

                var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

                return JsonConvert.DeserializeObject<HotelPriceDto>(result, settings);
            }

            return null;
        }
    }
}
