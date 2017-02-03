using core.customers.dtos;

namespace core.customers.services
{
    public interface ICustomerService
    {
        CustomerDto InsertUpdateCustomer(string reference, Customer customer);
        CustomerDto ReturnCustomerByReference(string guid);
        CustomerDto ReturnCustomerByToken(string token);
        CustomerDto ReturnCustomerByEmailAddress(string emailAddress);
        CustomerDto ReturnCustomerByEmailAddressPassword(string emailAddress, string password);

        void SendCustomerForgotPasswordEmail(int id);
        void SendCustomerWelcomeEmail(int id);
    }
}
