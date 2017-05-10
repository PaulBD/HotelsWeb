using Newtonsoft.Json;
using System.Collections.Generic;

namespace core.extras.dtos
{
    public class ParkingAttributes
    {
        public string Product { get; set; }
        public int RequestCode { get; set; }
        public string Result { get; set; }
        public string Expires { get; set; }
    }

    public class ParkingRequestFlags
    {
        public int Registration { get; set; }
        public int CarMake { get; set; }
        public int CarModel { get; set; }
        public int CarColour { get; set; }
        public int ReturnFlight { get; set; }
        public int? OutFlight { get; set; }
        public int? OutTerminal { get; set; }
        public int? ReturnTerminal { get; set; }
        public int? Destination { get; set; }
        public int? MobileNum { get; set; }
    }

    public class ParkingFilter
    {
        public int Park_And_Ride { get; set; }
        public int Meet_And_Greet { get; set; }
        public int Car_Parked_For_You { get; set; }
        public int Lead_Time { get; set; }
        public string Terminal { get; set; }
        public int? On_Airport { get; set; }
    }

    public class ParkingImage
    {
        public string Alt { get; set; }
        public string Src { get; set; }
    }

    public class ParkingImagesList
    {
        public ParkingImagesList()
        {
            image = new List<ParkingImage>();
        }

        public List<ParkingImage> image { get; set; }
    }

    public class CarPark
    {
        public CarPark()
        {
            RequestFlags = new ParkingRequestFlags();
            Filter = new ParkingFilter();
        }
        public double TotalPrice { get; set; }
        public double NonDiscPrice { get; set; }
        public double GatePrice { get; set; }
        public ParkingRequestFlags RequestFlags { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ParkingFilter Filter { get; set; }
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
        public string Introduction { get; set; }
        public string TripapCarparkSellpoint { get; set; }
        public string TripappTransferTip { get; set; }
        public string Star_Rating { get; set; }
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
        public ParkingImagesList Images { get; set; }
        public string BookingURL { get; set; }
        public string MoreInfoURL { get; set; }
        public List<object> Attributes { get; set; }
        public bool Advance_Purchase { get; set; }
    }

    public class CancellationWaiver
    {
        public double Waiver { get; set; }
    }

    public class ParkingPricing
    {
        public ParkingPricing()
        {
            CancellationWaiver = new List<dtos.CancellationWaiver>();
        }

        public double CCardSurchargePercent { get; set; }
        public string CCardSurchargeMin { get; set; }
        public int CCardSurchargeMax { get; set; }
        public string DCardSurchargePercent { get; set; }
        public string DCardSurchargeMin { get; set; }
        public string DCardSurchargeMax { get; set; }
        public List<CancellationWaiver> CancellationWaiver { get; set; }
    }

    public class ParkingRequest
    {
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public string DepartDate { get; set; }
        public string DepartTime { get; set; }
        public string Key { get; set; }
        public string Token { get; set; }
        public int v { get; set; }
        public string Format { get; set; }
    }

    public class ParkingAPIHeader
    {
        public ParkingAPIHeader()
        {
            Request = new ParkingRequest();
        }

        public ParkingRequest Request { get; set; }
    }

	public class ParkingApiError
	{
		public string Code { get; set; }
		public string Message { get; set; }
	}

	public class ParkingAPIReply
	{
        public ParkingAPIReply()
        {
            Attributes = new ParkingAttributes();
            API_Header = new ParkingAPIHeader();
            CarPark = new List<CarPark>();
            Pricing = new ParkingPricing();
            Error = new ParkingApiError();
        }

        public ParkingAttributes Attributes { get; set; }
        public List<CarPark> CarPark { get; set; }
        public ParkingPricing Pricing { get; set; }
		public ParkingAPIHeader API_Header { get; set; }
		public ParkingApiError Error { get; set; }
    }

    public class AirportParkingResponseDto
    {
        public AirportParkingResponseDto()
        {
            API_Reply = new ParkingAPIReply();
        }

        public ParkingAPIReply API_Reply { get; set; }
    }
}
