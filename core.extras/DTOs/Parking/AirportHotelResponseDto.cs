using System.Collections.Generic;

namespace core.extras.dtos
{
    public class AirportHotelAttributes
    {
        public string Product { get; set; }
        public int RequestCode { get; set; }
        public string Result { get; set; }
        public bool cached { get; set; }
        public string expires { get; set; }
    }

    public class AirportHotelRequestFlags
    {
        public int CarColour { get; set; }
        public int CarDropoffTime { get; set; }
        public int CarMake { get; set; }
        public int CarModel { get; set; }
        public int CarPickupTime { get; set; }
        public int CreditCard { get; set; }
        public int Registration { get; set; }
        public int ReturnFlight { get; set; }
        public int? ParkingIncludesArrival { get; set; }
    }

    public class AirportHotelFilter
    {
        public string day_use_only { get; set; }
        public int car_park_assoc { get; set; }
        public int? hotel_arrival_parking_ppts { get; set; }
        public int? hotel_return_parking_ppts { get; set; }
    }

    public class Hotel
    {
        public Hotel()
        {
            Attributes = new List<object>();
            RequestFlags = new AirportHotelRequestFlags();
            Filter = new AirportHotelFilter();
        }

        public List<object> Attributes { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string BookingURL { get; set; }
        public string MoreInfoURL { get; set; }
        public double _latitude { get; set; }
        public double _longitude { get; set; }
        public AirportHotelRequestFlags RequestFlags { get; set; }
        public double Price { get; set; }
        public double PriceWithSurcharge { get; set; }
        public int Adults { get; set; }
        public string Children { get; set; }
        public string RoomCode { get; set; }
        public string BoardBasis { get; set; }
        public int ParkingDays { get; set; }
        public double NonDiscPrice { get; set; }
        public string CarPark { get; set; }
        public string car_park_assoc { get; set; }
        public int single_car_only { get; set; }
        public int noncancellable_nonrefundable { get; set; }
        public AirportHotelFilter Filter { get; set; }
        public bool advance_purchase { get; set; }
        public int? parking_includes_arrival { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
        public string Sellpoint_Location { get; set; }
        public string Sellpoint_Parking { get; set; }
        public string Sellpoint_Security { get; set; }
        public string Sellpoint_Transfers { get; set; }
        public string Transfers { get; set; }
        public string TripappImages { get; set; }
        public string[] TripappImagesList
        {
            get
            {
                if (!string.IsNullOrEmpty(TripappImages))
                {
                    return TripappImages.Split(';');
                }

                return null;
            }
        }
    }

    public class AirportHotelPricing
    {
        public int CCardSurchargePercent { get; set; }
        public string CCardSurchargeMin { get; set; }
        public int CCardSurchargeMax { get; set; }
        public string DCardSurchargePercent { get; set; }
        public string DCardSurchargeMin { get; set; }
        public string DCardSurchargeMax { get; set; }
        public double WaiverValue { get; set; }
    }

    public class AirportHotelRequest
    {
        public string Initials { get; set; }
        public string key { get; set; }
        public string token { get; set; }
        public string ArrivalDate { get; set; }
        public int Nights { get; set; }
        public int ParkingDays { get; set; }
        public string RoomType { get; set; }
        public string format { get; set; }
    }

    public class AirportHotelApiHeader
    {
        public AirportHotelRequest Request { get; set; }
    }

    public class AirportHotelApiReply
    {
        public AirportHotelApiReply()
        {
            Attributes = new AirportHotelAttributes();
            Hotel = new List<Hotel>();
            Pricing = new AirportHotelPricing();
            API_Header = new AirportHotelApiHeader();
        }

        public AirportHotelAttributes Attributes { get; set; }
        public List<Hotel> Hotel { get; set; }
        public AirportHotelPricing Pricing { get; set; }
        public string SepaID { get; set; }
        public AirportHotelApiHeader API_Header { get; set; }
    }

    public class AirportHotelResponseDto
    {
        public AirportHotelApiReply API_Reply { get; set; }
    }
}
