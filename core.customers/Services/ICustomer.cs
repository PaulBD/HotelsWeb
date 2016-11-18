
namespace core.customers.services
{
    public interface ICustomer
    {
        void InsertNewCustomer();
        void ReturnCustomerById(int id);
        void ReturnCustomerByUsernamePassword(string username, string password);

        void SendCustomerForgotPasswordEmail(int id);
        void SendCustomerWelcomeEmail(int id);
    }
}
