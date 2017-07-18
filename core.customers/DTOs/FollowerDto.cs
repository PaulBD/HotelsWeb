using System;
using System.Collections.Generic;

namespace core.customers.dtos
{
    public class FollowerDto
    {
        public string Bio { get; set; }
        public string Name { get; set; }
        public string ProfileUrl { get; set; }
        public string ImageUrl { get; set; }
        public string CurrentLocation { get; set; }
        public string BackgroundImageUrl { get; set; }

    }

    public class FollowerListDto
    {
        public FollowerListDto()
        {
            Followers = new List<FollowerListDto>();
        }

        public List<FollowerListDto> Followers { get; set; }
    }
}
