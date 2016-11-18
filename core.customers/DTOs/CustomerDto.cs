using core.customers.enums;
using System;
using System.Collections.Generic;

namespace core.customers.dtos
{
    public class CustomerDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> EmailAddressList { get; set; }
        public List<string> CookieList { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }

        public List<BehaviourDto> BehaviourList { get; set; }
        public List<FriendListDto> FriendList { get; set; }
    }

    public class FriendListDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
    }

    public class BehaviourDto
    {
        public DateTime Date { get; set; }
        public BehaviourType Type { get; set; }
    }

    public class CountryDto
    {

    }

    public class CityDto
    {

    }

    public class PlaceDto
    {

    }

    public class HotelDto
    {

    }
}
