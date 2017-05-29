namespace core.customers.dtos
{
    public class AuthorizationDto
    {
        public string Token { get; set; }
		public string UserName { get; set; }
		public string UserId { get; set; }
        public string UserImage { get; set; }
        public string BaseUrl { get; set; }
	}

	public class TokenDto
	{
		public string EmailAddress { get; set; }
        public string Reference { get; set; }
	}
}
