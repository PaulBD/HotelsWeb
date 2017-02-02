
using System.Collections.Generic;

namespace core.places.dtos
{

    public class Coordinates
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class HoursDto
    {
        public List<List<string>> Sunday { get; set; }
        public List<List<string>> Saturday { get; set; }
        public List<List<string>> Tuesday { get; set; }
        public List<List<string>> Friday { get; set; }
        public List<List<string>> Thursday { get; set; }
        public List<List<string>> Wednesday { get; set; }
        public List<List<string>> Monday { get; set; }
    }
}
