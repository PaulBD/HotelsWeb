using System.Collections.Generic;

namespace core.extras.dtos
{
    public class ParkingAttributesDto
    {
        public string Product { get; set; }
        public int RequestCode { get; set; }
        public string Result { get; set; }
        public string expires { get; set; }
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
        public int park_and_ride { get; set; }
        public object meet_and_greet { get; set; }
        public int car_parked_for_you { get; set; }
        public int lead_time { get; set; }
        public string terminal { get; set; }
        public int? on_airport { get; set; }
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
        public object GatePrice { get; set; }
        public ParkingRequestFlagsDto RequestFlags { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ParkingFilterDto Filter { get; set; }
        public double _latitude { get; set; }
        public double _longitude { get; set; }
        public string equivalent_product { get; set; }
        public List<string> _a_equivalent_product { get; set; }
        public string BookingURL { get; set; }
        public string MoreInfoURL { get; set; }
        public List<object> Attributes { get; set; }
        public int? advance_purchase { get; set; }
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
        public int ArrivalTime { get; set; }
        public string DepartDate { get; set; }
        public int DepartTime { get; set; }
        public string key { get; set; }
        public string token { get; set; }
        public int v { get; set; }
        public string format { get; set; }
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

    public class ParkingDto
    {
        public ParkingDto()
        {
            API_Reply = new APIReply();
        }

        public APIReply API_Reply { get; set; }
    }
}
