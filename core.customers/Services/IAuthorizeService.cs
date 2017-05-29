using core.customers.dtos;

namespace core.customers.services
{
    public interface IAuthorizeService
    {
        string AssignToken(string emailAddress, string reference);
        TokenDto AuthorizeCustomer(string token);
    }
}
