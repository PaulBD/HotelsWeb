namespace core.customers.services
{
    public interface ICustomerService
    {
        void InsertNewCustomer();
        void ReturnCustomerById(int id);
        void ReturnCustomerByUsernamePassword(string username, string password);

        void SendCustomerForgotPasswordEmail(int id);
        void SendCustomerWelcomeEmail(int id);
    }
}
