using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Configuration;
using System.Linq;
using api.laterooms.co.uk.Exceptions;
using dtos.laterooms.co.uk;
using api.laterooms.co.uk.Enums;
using System.Xml;
using Newtonsoft.Json;

namespace api.laterooms.co.uk.Services
{
    public class LateRoomsApiService : ILateRoomsApiService
    {
        private readonly HttpClient _client;
        private readonly string _affiliateId;
        private readonly string _currency;
        private readonly string _startDate;
        private readonly int _nights;
        
        public LateRoomsApiService()
        {
            _affiliateId = ConfigurationManager.AppSettings["hotels.laterooms.affiliateId"];
            

            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["hotels.laterooms.baseUrl"])
            };
        }

        /// <summary>
        /// Return selected Hotels
        /// </summary>
        /// <example>xmlfeed.laterooms.com/index.aspx?aid=100&amp;rtype=3&amp;hids=73737,178646&amp;cur=eur&amp;sdate=2010-02-21&amp;nights=2</example>
        public LateRoomsDto SearchSelectedHotels(List<int> hids, LateroomsCurrency currency, string startDate, int nights)
        {
            return LateRoomsRequest("index.aspx?aid=" + _affiliateId + "&rtype=3&hids=" + ReturnHids(hids) + "&cur=" + currency + "&sdate=" + startDate + "&nights=" + nights);
        }

        /// <summary>
        /// Return Hotels By Keyword
        /// </summary>
        /// <example>//xmlfeed.laterooms.com/index.aspx?aid=100&amp;rtype=4&amp;kword=leeds&amp;sdate=2010-03-18&amp;nights=3</example>
        public LateRoomsDto SearchHotelsByKeyword(string keyword, LateroomsCurrency currency, string startDate, int nights)
        {
            return LateRoomsRequest("index.aspx?aid=" + _affiliateId + "&rtype=4&kword=" + keyword + "&cur=" + ReturnCurrency(currency) + "&sdate=" + startDate + "&nights=" + nights);
        }

        /// <summary>
        /// Return Live Prices
        /// </summary>
        /// <example>//xmlfeed.laterooms.com/index.aspx?aid=100&amp;rtype=7&amp;hid=12345&amp;sdate=2010-03-18&amp;nights=3</example>
        public LivePricesDto SearchLivePrices(string hid, LateroomsCurrency currency, string startDate, int nights)
        {
            return LivePricesRequest("index.aspx?aid=" + _affiliateId + "&rtype=7&hids=" + hid + "&cur=" + ReturnCurrency(currency) + "&sdate=" + startDate + "&nights=" + nights);
        }

        /// <summary>
        /// Return Currency
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        private string ReturnCurrency(LateroomsCurrency currency)
        {
            if (currency == LateroomsCurrency.Aud)
            {
                return "AUD";
            }
            if (currency == LateroomsCurrency.Eur)
            {
                return "EUR";
            }
            if (currency == LateroomsCurrency.Usd)
            {
                return "USD";
            }
            if (currency == LateroomsCurrency.Cad)
            {
                return "CAD";
            }

            return "GBP";
        }

        /// <summary>
        /// Late Rooms request
        /// </summary>
        private LateRoomsDto LateRoomsRequest(string request)
        {
            LateRoomsDto dto = null;
            var doc = new XmlDocument();

            try
            {
                var response = _client.GetAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    result = NormaliseResponse(result);

                    doc.LoadXml(result);
                    result = JsonConvert.SerializeXmlNode(doc);

                    var settings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
                    dto = JsonConvert.DeserializeObject<LateRoomsDto>(result, settings);
                }
            }
            catch (Exception ex)
            {
                throw new LateRoomsApiExcepetion(ex, string.Format("An error has occured contacting the LateRooms.com - {0}", request));
            }


            return dto;
        }

        /// <summary>
        /// Late Rooms request
        /// </summary>
        private LivePricesDto LivePricesRequest(string request)
        {
            LivePricesDto dto = null;
            var doc = new XmlDocument();

            try
            {
                var response = _client.GetAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    result = NormaliseResponse(result);

                    doc.LoadXml(result);
                    result = JsonConvert.SerializeXmlNode(doc);

                    var settings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};

                    dto = JsonConvert.DeserializeObject<LivePricesDto>(result, settings);
                }
            }
            catch (Exception ex)
            {
                throw new LateRoomsApiExcepetion(ex, string.Format("An error has occured contacting the LateRooms.com for Live Prices - {0}", request));
            }


            return dto;
        }

        /// <summary>
        /// Utility method for creating a comma delimited string form list int
        /// </summary>
        private string ReturnHids(List<int> hids)
        {
            string s = hids.Aggregate("", (current, i) => current + (i.ToString() + ","));

            if (s.EndsWith(","))
            {
                s = s.Remove(s.Length - 1, 1);
            }

            return s;
        }

        /// <summary>
        /// Normalise the XML Response
        /// </summary>
        private string NormaliseResponse(string result)
        {
            //result = result.Replace("http://www.w3.org/2001/XMLSchema-instance", "http://james.newtonking.com/projects/json");

            // root
            result = result.Replace("<lr_rates", "<lr_rates xmlns:json='http://james.newtonking.com/projects/json' "); // Need to miss the trailing > as we have a namespace here
            result = result.Replace("<root", "<hotel_search xmlns:json='http://james.newtonking.com/projects/json' "); // Need to miss the trailing > as we have a namespace here
            result = result.Replace("</root>", "</hotel_search>");

            // Images - Detailed view contains an image list rather than just one image
            if (!result.Contains("<url>"))
            {
                result = result.Replace("<images>", "<image>");
                result = result.Replace("</images>", "</image>");
            }

            // Force a json Array
            result = result.Replace("<rate>", "<rate json:Array='true'>");
            //result = result.Replace("<hotel_rooms>", "<hotel_rooms json:Array='true'>");

            //credit cards
            result = result.Replace("<accepted_credit_cards>", "<hotel_credit_cards>");
            result = result.Replace("</accepted_credit_cards>", "</hotel_credit_cards>");
            result = result.Replace("<accepted_payment_credit_cards>", "<hotel_credit_cards_payment>");
            result = result.Replace("</accepted_payment_credit_cards>", "</hotel_credit_cards_payment>");

            result = result.Replace("xsi:nil=\"true\"", "");
                
            //facilities
            result = result.Replace("<hotel_facilities>", "<facilities>");
            result = result.Replace("</hotel_facilities>", "</facilities>");
            result = result.Replace("<facility>", "<facility json:Array='true'>");

            //Appeals
            result = result.Replace("<hotel_appeals>", "<appeals>");
            result = result.Replace("</hotel_appeals>", "</appeals>");
            result = result.Replace("<appeal>", "<appeal json:Array='true'>");

            result = result.Replace("<hotel_appeals>", "<hotel_appeals json:Array='true'>");

            return result;
        }
    }
}
