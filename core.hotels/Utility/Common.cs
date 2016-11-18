using core.hotels.enums;

namespace core.hotels.utility
{
    public class Common
    {
        /// <summary>
        /// Return Currency
        /// </summary>
        public string ReturnCurrency(Currency currency)
        {
            if (currency == Currency.Aud)
            {
                return "AUD";
            }
            if (currency == Currency.Eur)
            {
                return "EUR";
            }
            if (currency == Currency.Usd)
            {
                return "USD";
            }
            if (currency == Currency.Cad)
            {
                return "CAD";
            }

            return "GBP";
        }


        /// <summary>
        /// Normalise the XML Response
        /// </summary>
        public string NormaliseResponse(string result)
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
