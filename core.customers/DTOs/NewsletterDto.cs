using System;
using System.Collections.Generic;

namespace core.customers.dtos
{
    public class NewsletterDto
    {
        public string EmailAddress { get; set; }
        public DateTime DateAdded { get; set; }
    }

    public class NewsletterListDto
    {
        public NewsletterListDto()
        {
            NewsletterDto = new List<NewsletterDto>();
        }

        public string Reference { get { return "NewsletterList"; } }
        public List<NewsletterDto> NewsletterDto { get; set; }
    }

    public class Newsletter
    {
        public Newsletter()
        {
            TriperooCustomers = new NewsletterListDto();
        }

        public NewsletterListDto TriperooCustomers { get; set; }
    }
}
