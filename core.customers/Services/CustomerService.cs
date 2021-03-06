﻿using core.customers.dtos;
using library.couchbase;
using System;
using System.Collections.Generic;
using core.places.dtos;
using library.common;
using System.Configuration;

namespace core.customers.services
{
    public class CustomerService : ICustomerService
    {
        private CouchBaseHelper _couchbaseHelper;
        private EncryptionService _encryptionService;
        protected string _password;
        private readonly string _bucketName = "TriperooCustomers";

        public CustomerService()
        {
            _couchbaseHelper = new CouchBaseHelper();
            _encryptionService = new EncryptionService();
            _password = ConfigurationManager.AppSettings["encryption.password"];
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
        /// Return Customer by encrypted guid
        /// </summary>
        public CustomerDto ReturnCustomerByEncryptedGuid(string encryptedGuid)
        {
            var customerGuid = _encryptionService.DecryptText(encryptedGuid, _password);

            return ReturnCustomerByReference(customerGuid);
        }

        /// <summary>
        /// Return Customer by email address & password
        /// </summary>
        public CustomerDto ReturnCustomerByEmailAddressPassword(string emailAddress, string password)
        {
            var q = "SELECT * FROM " + _bucketName + " WHERE profile.emailAddress = '" + emailAddress + "' AND profile.pass = '" + password + "'";
            return ProcessQuery(q);
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
