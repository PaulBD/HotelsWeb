using System.Collections.Generic;

namespace core.places.dtos
{
    public class ProductName
    {
        public string en { get; set; }
    }

    public class Introduction
    {
        public string en { get; set; }
    }

    public class ProductText
    {
        public string en { get; set; }
    }

    public class ProductImage
    {
        public string ThumbnailURL { get; set; }
        public string ImageURL { get; set; }
    }

    public class Destination
    {
        public string ID { get; set; }
        public string Continent { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string IATACode { get; set; }
    }

    public class ProductCategory
    {
        public string Group { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
    }

    public class ProductCategories
    {
        public List<ProductCategory> ProductCategory { get; set; }
    }

    public class ProductURLs
    {
        public string ProductURL { get; set; }
    }

    public class Pricing
    {
        public string PriceAUD { get; set; }
        public string PriceNZD { get; set; }
        public string PriceEUR { get; set; }
        public string PriceGBP { get; set; }
        public string PriceUSD { get; set; }
        public string PriceCAD { get; set; }
        public string PriceCHF { get; set; }
        public string PriceNOK { get; set; }
        public string PriceJPY { get; set; }
        public string PriceSEK { get; set; }
        public string PriceHKD { get; set; }
        public string PriceSGD { get; set; }
        public string PriceZAR { get; set; }
    }

    public class AttractionDto
    {
        public string Rank { get; set; }
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
        public ProductName ProductName { get; set; }
        public Introduction Introduction { get; set; }
        public ProductText ProductText { get; set; }
        public object SpecialDescription { get; set; }
        public string Special { get; set; }
        public string Duration { get; set; }
        public string Commences { get; set; }
        public ProductImage ProductImage { get; set; }
        public Destination Destination { get; set; }
        public ProductCategories ProductCategories { get; set; }
        public ProductURLs ProductURLs { get; set; }
        public Pricing Pricing { get; set; }
        public object ProductStarRating { get; set; }
        public string BookingType { get; set; }
        public string VoucherOption { get; set; }
    }

    public class AttractionListDto
    {
        public AttractionListDto()
        {
            Attractions = new List<AttractionDto>();
            Categories = new List<CategoryDto>();
        }

        public IList<AttractionDto> Attractions { get; set; }
        public IList<CategoryDto> Categories { get; set; }
        public int LocationCount { get; set; }
    }
}
