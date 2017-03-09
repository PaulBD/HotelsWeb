using core.customers.dtos;
using library.couchbase;
using System;

namespace core.customers.services
{
    public class CustomerService : ICustomerService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooCustomers";

        public CustomerService()
        {
            _couchbaseHelper = new CouchBaseHelper();
        }

        /// <summary>
        /// Insert or Update customer
        /// </summary>
        public CustomerDto InsertUpdateCustomer(string reference, Customer customer)
        {
            _couchbaseHelper.AddRecordToCouchbase(reference, customer, _bucketName);

            return new CustomerDto { TriperooCustomers = customer };
        }

        /// <summary>
        /// Return Customer by reference
        /// </summary>
        public CustomerDto ReturnCustomerByReference(string guid)
        {
            if (!guid.Contains("customer"))
            {
                guid = "customer:" + guid;
            }

            var q = "SELECT * FROM " + _bucketName + " WHERE customerReference = '" + guid + "'";
            return ProcessQuery(q);
        }

        /// <summary>
        /// Return Customer by token
        /// </summary>
        public CustomerDto ReturnCustomerByToken(string token)
        {
            var q = "SELECT * FROM " + _bucketName + " WHERE token = '" + token + "'";
            return ProcessQuery(q);
        }

        /// <summary>
        /// Return Customer by email address
        /// </summary>
        public CustomerDto ReturnCustomerByEmailAddress(string emailAddress)
        {
            var q = "SELECT * FROM " + _bucketName + " WHERE profile.emailAddress = '" + emailAddress + "'";
            return ProcessQuery(q);
        }

        /// <summary>
        /// Return Customer by email address & password
        /// </summary>
        public CustomerDto ReturnCustomerByEmailAddressPassword(string emailAddress, string password)
        {
            var q = "SELECT * FROM " + _bucketName + " WHERE profile.emailAddress = '" + emailAddress + "' AND profile.pass = '" + password + "'";
            return ProcessQuery(q);
        }

        public void SendCustomerForgotPasswordEmail(int id)
        {
            throw new NotImplementedException();
        }

        public void SendCustomerWelcomeEmail(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Process Query
        /// </summary>
        private CustomerDto ProcessQuery(string q)
        {
            var result = _couchbaseHelper.ReturnQuery<CustomerDto>(q, _bucketName);

            if (result.Count > 0)
            {
                return result[0];
            }

            return null;
        }

        /// <summary>
        /// Insert Newsletter
        /// </summary>
        public NewsletterDto InsertNewsletter(NewsletterDto newsletter)
        {
            var result = _couchbaseHelper.ReturnQuery<Newsletter>("SELECT * FROM " + _bucketName + " WHERE reference = 'NewsletterList'", _bucketName);

            var list = new NewsletterListDto();
            list.NewsletterDto.Add(newsletter);

            if (result.Count > 0)
            {
                list.NewsletterDto.AddRange(result[0].TriperooCustomers.NewsletterDto);
            }

            _couchbaseHelper.AddRecordToCouchbase("Newsletter", list, _bucketName);
            
            return newsletter;
        }
    }
}
