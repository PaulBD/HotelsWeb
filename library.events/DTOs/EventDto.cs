using System.Collections.Generic;
using System.Web;

namespace library.events.dtos
{
    public class LinkDto
    {
        public string Time { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
    }

    public class Links
    {
        public List<LinkDto> link { get; set; }
    }

    public class Performer
    {
        public string creator { get; set; }
        public string linker { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string id { get; set; }
        public string short_bio { get; set; }
    }

    public class Performers
    {
        public Performer performer { get; set; }
    }

    public class CategoryDto
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class CategoriesDto
    {
        public List<CategoryDto> Category { get; set; }
    }

    public class TicketLinksDto
    {
        public string Provider { get; set; }
        public string Url { get; set; }
        public object Description { get; set; }
    }

    public class Tickets
    {
        public List<TicketLinksDto> Link { get; set; }
    }

    public class Block250
    {
        public string width { get; set; }
        public string url { get; set; }
        public string height { get; set; }
    }

    public class Image
    {
        public Block250 block250 { get; set; }
    }

    public class Event
    {
        public object watching_count { get; set; }
        public string olson_path { get; set; }
        public object calendar_count { get; set; }
        public object comment_count { get; set; }
        public string region_abbr { get; set; }
        public object postal_code { get; set; }
        public object going_count { get; set; }
        public string all_day { get; set; }
        public string latitude { get; set; }
        public object groups { get; set; }
        public string url { get; set; }
        public string id { get; set; }
        public string privacy { get; set; }
        public string city_name { get; set; }
        public object link_count { get; set; }
        public string longitude { get; set; }
        public string country_name { get; set; }
        public string country_abbr { get; set; }
        public string region_name { get; set; }
        public string price { get; set; }
        public string start_time { get; set; }
        public object tz_id { get; set; }
        public string description { get; set; }
        public string descriptionDecoded
        {
            get
            {
                return HttpUtility.HtmlDecode(description);
            }
        }
        public string modified { get; set; }
        public string venue_display { get; set; }
        public object tz_country { get; set; }
		public string title { get; set; }
        public string friendlyTitle
        {
            get
            {
                return title.Replace("'","").Replace(" ", "-").ToLower();
            }
        }
        public string venue_address { get; set; }
        public string geocode_type { get; set; }
        public object tz_olson_path { get; set; }
        public object recur_string { get; set; }
        public object calendars { get; set; }
        public string owner { get; set; }
        public object going { get; set; }
        public string country_abbr2 { get; set; }
        public Image image { get; set; }
        public string created { get; set; }
        public string venue_id { get; set; }
        public object tz_city { get; set; }
        public string stop_time { get; set; }
        public string venue_name { get; set; }
        public string venue_url { get; set; }
        public CategoriesDto categories { get; set; }
        public Tickets tickets { get; set; }
        public Links links { get; set; }
        public Performers performers { get; set; }
    }

    public class EventsDto
    {
        public List<Event> Event { get; set; }
    }

    public class EventDto
    {
        public object last_item { get; set; }
        public string total_items { get; set; }
        public object first_item { get; set; }
        public string page_number { get; set; }
        public string page_size { get; set; }
        public object page_items { get; set; }
        public string search_time { get; set; }
        public string page_count { get; set; }
        public EventsDto events { get; set; }
    }
}
