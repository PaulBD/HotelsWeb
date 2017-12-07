using core.customers.dtos;
using core.places.dtos;

namespace core.customers.services
{
    public interface ICustomerService
    {
        NewsletterDto InsertNewsletter(NewsletterDto newsletter);
        CustomerDto InsertUpdateCustomer(string reference, Customer customer);
        CustomerDto ReturnCustomerByReference(string guid);
        CustomerDto ReturnCustomerByEncryptedGuid(string encryptedGuid);
        CustomerDto ReturnCustomerByToken(string token);
        CustomerDto ReturnCustomerByEmailAddress(string emailAddress);
        CustomerDto ReturnCustomerByEmailAddressPassword(string emailAddress, string password);
    }
}
