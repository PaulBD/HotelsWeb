using System.Collections.Generic;

namespace core.places.dtos
{
    public class LocationDto
    {
        public string Name { get; set; }
        public string NameShort { get; set; }
        public int Priority { get; set; }
        public string Search { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public int InventoryReference { get; set; }
    }

    public class AutocompleteDto
    {
        public AutocompleteDto()
        {
            Locations = new List<LocationDto>();
        }

        public List<LocationDto> Locations { get; set; }
    }
}
