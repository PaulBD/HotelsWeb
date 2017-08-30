using System.Collections.Generic;

namespace core.hotels.dtos
{
    public class HotelDetailSummary
    {
        public string order { get; set; }
        public int hotelId { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string countryCode { get; set; }
        public int propertyCategory { get; set; }
        public double hotelRating { get; set; }
        public string hotelRatingDisplay { get; set; }
        public double tripAdvisorRating { get; set; }
        public int tripAdvisorReviewCount { get; set; }
        public string tripAdvisorRatingUrl { get; set; }
        public string locationDescription { get; set; }
        public double highRate { get; set; }
        public double lowRate { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class HotelDetails
    {
        public int numberOfRooms { get; set; }
        public int numberOfFloors { get; set; }
        public string checkInTime { get; set; }
        public string checkInEndTime { get; set; }
        public string checkInMinAge { get; set; }
        public string checkOutTime { get; set; }
        public string propertyInformation { get; set; }
        public string areaInformation { get; set; }
        public string propertyDescription { get; set; }
        public string hotelPolicy { get; set; }
        public string roomInformation { get; set; }
        public string checkInInstructions { get; set; }
        public string nationalRatingsDescription { get; set; }
        public string knowBeforeYouGoDescription { get; set; }
        public string roomFeesDescription { get; set; }
        public string locationDescription { get; set; }
        public string diningDescription { get; set; }
        public string amenitiesDescription { get; set; }
        public string businessAmenitiesDescription { get; set; }
        public string roomDetailDescription { get; set; }
    }

    public class Supplier
    {
        public int id { get; set; }
        public string chainCode { get; set; }
    }

    public class Suppliers
    {
        public string size { get; set; }
        public Supplier supplier { get; set; }
    }

    public class RoomAmenity
    {
        public int amenityId { get; set; }
        public string amenity { get; set; }
    }

    public class RoomAmenities
    {
        public string size { get; set; }
        public List<RoomAmenity> roomAmenity { get; set; }
    }

    public class RoomType
    {
        public int roomTypeId { get; set; }
        public string roomCode { get; set; }
        public string description { get; set; }
        public string descriptionLong { get; set; }
        public RoomAmenities roomAmenities { get; set; }
    }

    public class RoomTypes
    {
        public string size { get; set; }
        public List<RoomType> roomType { get; set; }
    }

    public class PropertyAmenity
    {
        public int amenityId { get; set; }
        public string amenity { get; set; }
    }

    public class PropertyAmenities
    {
        public string size { get; set; }
        public List<PropertyAmenity> PropertyAmenity { get; set; }
    }

    public class HotelImage
    {
        public int hotelImageId { get; set; }
        public string name { get; set; }
        public int category { get; set; }
        public int type { get; set; }
        public string caption { get; set; }
        public string url { get; set; }
        public string thumbnailUrl { get; set; }
        public int supplierId { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int byteSize { get; set; }
        public string highResolutionUrl { get; set; }
        public bool heroImage { get; set; }
    }

    public class HotelImages
    {
        public string size { get; set; }
        public List<HotelImage> HotelImage { get; set; }
    }

    public class HotelInformationResponse
    {
        public int hotelId { get; set; }
        public string customerSessionId { get; set; }
        public HotelDetailSummary HotelSummary { get; set; }
        public HotelDetails HotelDetails { get; set; }
        public Suppliers Suppliers { get; set; }
        public RoomTypes RoomTypes { get; set; }
        public PropertyAmenities PropertyAmenities { get; set; }
        public HotelImages HotelImages { get; set; }
    }

    public class HotelDto
    {
        public HotelInformationResponse HotelInformationResponse { get; set; }
    }
}
