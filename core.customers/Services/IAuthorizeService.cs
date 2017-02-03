
namespace core.customers.services
{
    public interface IAuthorizeService
    {
        string AssignToken(string emailAddress, string reference);
        string AuthorizeCustomer(string token);
    }
}
