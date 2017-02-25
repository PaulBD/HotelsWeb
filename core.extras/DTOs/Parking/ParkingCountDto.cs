namespace core.extras.dtos
{
    public class ParkingCountRequestDto
    {
        public string ArrivalDate { get; set; }
        public string DepartDate { get; set; }
        public string key { get; set; }
        public string token { get; set; }
        public string Initials { get; set; }
        public int v { get; set; }
        public string format { get; set; }
    }

    public class ParkingCountAPIHeaderDto
    {
        public ParkingCountAPIHeaderDto()
        {
            Request = new ParkingCountRequestDto();
        }

        public ParkingCountRequestDto Request { get; set; }
    }

    public class ParkingCountSpacesDto
    {
        public int Total { get; set; }
        public int Left { get; set; }
    }

    public class ParkingSpacesCountDto
    {
        public ParkingSpacesCountDto()
        {
            Spaces = new ParkingCountSpacesDto();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public ParkingCountSpacesDto Spaces { get; set; }
    }

    public class ParkingCountAPIReplyDto
    {
        public ParkingCountAPIReplyDto()
        {
            API_Header = new ParkingCountAPIHeaderDto();
            CarPark = new ParkingSpacesCountDto();
        }

        public ParkingCountAPIHeaderDto API_Header { get; set; }
        public ParkingSpacesCountDto CarPark { get; set; }
    }

    public class ParkingCountDto
    {
        public ParkingCountDto()
        {
            API_Reply = new ParkingCountAPIReplyDto();
        }

        public ParkingCountAPIReplyDto API_Reply { get; set; }
    }
}
