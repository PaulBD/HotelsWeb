using System.Collections.Generic;

namespace core.places.dtos
{
    public class Place
    {
        public string documentId { get; set; }
        public string name { get; set; }
        public string searchName { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public int priority { get; set; }
    }

    public class TriperooCommon
    {
        public TriperooCommon()
        {
            places = new List<Place>();
        }

        public int count { get; set; }
        public string letterIndex { get; set; }
        public List<Place> places { get; set; }
    }

    public class AutocompleteDto
    {
        public AutocompleteDto()
        {
            TriperooCommon = new TriperooCommon();
        }

        public TriperooCommon TriperooCommon { get; set; }
    }
}
