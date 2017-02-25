using System.Collections.Generic;

namespace core.extras.dtos
{
    public class ParkingUpgradeAttributesDto
    {
        public string Result { get; set; }
        public string expires { get; set; }
        public bool cached { get; set; }
        public string Product { get; set; }
        public string System { get; set; }
        public int Version { get; set; }
        public string Customer { get; set; }
        public int Session { get; set; }
        public int RequestCode { get; set; }
    }

    public class ParkingUpgradeItineraryDto
    {
        public string SiteCode { get; set; }
    }

    public class ParkingUpgradeFilterDto
    {
        public int? primary_for_product_typecarparks { get; set; }
        public int? wistia { get; set; }
    }

    public class ParkingUpgradeSupplementDto
    {
        public ParkingUpgradeSupplementDto()
        {
            Filter = new ParkingUpgradeFilterDto();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Per { get; set; }
        public string Canx { get; set; }
        public ParkingUpgradeFilterDto Filter { get; set; }
        public string description { get; set; }
        public string supplement_type { get; set; }
        public string Date { get; set; }
        public double AdPrice { get; set; }
        public double NonDiscAdPrice { get; set; }
        public string AdDiscAmt { get; set; }
        public double? ChPrice { get; set; }
        public double? NonDiscChPrice { get; set; }
        public string ChDiscAmt { get; set; }
        public int? Price { get; set; }
        public int? NonDiscPrice { get; set; }
        public string DiscAmt { get; set; }
    }

    public class ParkingUpgradeRequestDto
    {
        public int AdultsCount { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartDate { get; set; }
        public string key { get; set; }
        public int token { get; set; }
        public string format { get; set; }
    }

    public class ParkingUpgradeAPIHeaderDto
    {
        public ParkingUpgradeAPIHeaderDto()
        {
            Request = new ParkingUpgradeRequestDto();
        }

        public ParkingUpgradeRequestDto Request { get; set; }
    }

    public class ParkingUpgradeAPIReplyDto
    {
        public ParkingUpgradeAPIReplyDto()
        {
            Attributes = new ParkingUpgradeAttributesDto();
            Itinerary = new ParkingUpgradeItineraryDto();
            Supplement = new List<ParkingUpgradeSupplementDto>();
            API_Header = new ParkingUpgradeAPIHeaderDto();
        }

        public ParkingUpgradeAttributesDto Attributes { get; set; }
        public ParkingUpgradeItineraryDto Itinerary { get; set; }
        public List<ParkingUpgradeSupplementDto> Supplement { get; set; }
        public ParkingUpgradeAPIHeaderDto API_Header { get; set; }
    }

    public class ParkingUpgradesDto
    {
        public ParkingUpgradesDto()
        {
            API_Reply = new ParkingUpgradeAPIReplyDto();
        }

        public ParkingUpgradeAPIReplyDto API_Reply { get; set; }
    }
}
