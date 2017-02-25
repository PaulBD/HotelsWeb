
namespace core.extras.dtos
{
    public class TokenAttributesDto
    {
        public string Result { get; set; }
    }

    public class TokenRequestDto
    {
        public string Key { get; set; }
        public int V { get; set; }
        public string Format { get; set; }
    }

    public class TokenAPIHeaderDto
    {
        public TokenAPIHeaderDto()
        {
            Request = new TokenRequestDto();
        }

        public TokenRequestDto Request { get; set; }
    }

    public class TokenAPIReplyDto
    {
        public TokenAPIReplyDto()
        {
            Attributes = new TokenAttributesDto();
            API_Header = new TokenAPIHeaderDto();
        }

        public int Token { get; set; }
        public string BookingURL { get; set; }
        public TokenAttributesDto Attributes { get; set; }
        public TokenAPIHeaderDto API_Header { get; set; }
    }

    public class UserTokenDto
    {
        public UserTokenDto()
        {
            API_Reply = new TokenAPIReplyDto();
        }

        public TokenAPIReplyDto API_Reply { get; set; }
    }
}
