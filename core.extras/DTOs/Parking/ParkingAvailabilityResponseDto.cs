using Newtonsoft.Json;
using System.Collections.Generic;

namespace core.extras.dtos
{
    public class ParkingAttributesDto
    {
        public string Product { get; set; }
        public int RequestCode { get; set; }
        public string Result { get; set; }
        public string Expires { get; set; }
    }

    public class ParkingRequestFlagsDto
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

    public class ParkingFilterDto
    {
        public int Park_And_Ride { get; set; }
        public int Meet_And_Greet { get; set; }
        public int Car_Parked_For_You { get; set; }
        public int Lead_Time { get; set; }
        public string Terminal { get; set; }
        public int? On_Airport { get; set; }
    }

    public class ImageDto
    {
        public string Alt { get; set; }
        public string Src { get; set; }
    }

    public class ImagesListDto
    {
        public ImagesListDto()
        {
            image = new List<ImageDto>();
        }

        public List<ImageDto> image { get; set; }
    }

    public class CarParDto
    {
        public CarParDto()
        {
            RequestFlags = new ParkingRequestFlagsDto();
            Filter = new ParkingFilterDto();
        }
        public double TotalPrice { get; set; }
        public double NonDiscPrice { get; set; }
        public double GatePrice { get; set; }
        public ParkingRequestFlagsDto RequestFlags { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ParkingFilterDto Filter { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
        public string Sellpoint_Location { get; set; }
        public string Sellpoint_Parking { get; set; }
        public string Sellpoint_Security { get; set; }
        public string Sellpoint_Transfers { get; set; }
        public string Transfers { get; set; }
        public string Introduction { get; set; }
        public string TripappImages { get; set; }
        public string TripapCarparkSellpoint { get; set; }
        public string TripappTransferTip { get; set; }
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
        public ImagesListDto Images { get; set; }
        public string BookingURL { get; set; }
        public string MoreInfoURL { get; set; }
        public List<object> Attributes { get; set; }
        public bool Advance_Purchase { get; set; }
    }

    public class CancellationWaiver
    {
        public double Waiver { get; set; }
    }

    public class ParkingPricingDto
    {
        public ParkingPricingDto()
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

    public class ParkingRequestDto
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

    public class ParkingAPIHeaderDto
    {
        public ParkingAPIHeaderDto()
        {
            Request = new ParkingRequestDto();
        }

        public ParkingRequestDto Request { get; set; }
    }

    public class APIReply
    {
        public APIReply()
        {
            Attributes = new ParkingAttributesDto();
            API_Header = new ParkingAPIHeaderDto();
            CarPark = new List<CarParDto>();
            Pricing = new ParkingPricingDto();
        }

        public ParkingAttributesDto Attributes { get; set; }
        public List<CarParDto> CarPark { get; set; }
        public ParkingPricingDto Pricing { get; set; }
        public ParkingAPIHeaderDto API_Header { get; set; }
    }

    public class ParkingAvailabilityResponseDto
    {
        public ParkingAvailabilityResponseDto()
        {
            API_Reply = new APIReply();
        }

        public APIReply API_Reply { get; set; }
    }
}
