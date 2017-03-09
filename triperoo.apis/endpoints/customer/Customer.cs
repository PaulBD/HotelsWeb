using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Net;

namespace triperoo.apis.endpoints.customer
{
    #region Customer Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/customer", "GET")]
    [Route("/v1/customer", "PUT")]
    [Route("/v1/customer", "POST")]
    public class CustomerRequest
    {
        public Customer Customer { get; set; }
    }

    #endregion

    #region API logic

    public class CustomerApi : Service
    {
        private readonly ICustomerService _customerService;
        private readonly IAuthorizeService _authorizeService;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerApi(ICustomerService customerService, IAuthorizeService authorizeService)
        {
            _customerService = customerService;
            _authorizeService = authorizeService;
        }

        #region Return Customer By Reference

        /// <summary>
        /// Return Customer
        /// </summary>
        public object Get(CustomerRequest request)
        {
            CustomerDto response = new CustomerDto();

            try
            {
                var token = Request.Headers.Get("securityToken");

                if (token == null)
                {
                    throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
                }

                response = _customerService.ReturnCustomerByToken(token);

                if (response == null)
                {
                    throw new HttpError(HttpStatusCode.BadRequest, "Bad Request");
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        #endregion

        #region Insert Customer

        /// <summary>
        /// Insert Customer
        /// </summary>
        public object Post(CustomerRequest request)
        {
            var response = new CustomerDto();

            try
            {
                var guid = Guid.NewGuid().ToString();
                var customer = request.Customer;
                customer.DateCreated = DateTime.Now;
                customer.CustomerReference = "customer:" + guid;

                if (customer.Profile.FirstName.Length > 0)
                {
                    customer.Profile.ProfileUrl = "/profile/" + guid + "/" + request.Customer.Profile.FirstName.Replace(" ", "_").ToLower() + "-" + request.Customer.Profile.LastName.Replace(" ", "_").ToLower();
                }
                else
                {
                    customer.Profile.ProfileUrl = "/profile/" + guid + "/" + request.Customer.Profile.Name.Replace(" ", "_").ToLower();
                }

                if ((!string.IsNullOrEmpty(customer.Profile.EmailAddress)) && (!string.IsNullOrEmpty(customer.Profile.Pass)))
                {
                    customer.Token = _authorizeService.AssignToken(customer.Profile.EmailAddress, customer.CustomerReference);
                    customer.Profile.Pass = "";
                }

                response = _customerService.InsertUpdateCustomer(customer.CustomerReference, customer);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        #endregion

        #region Update Existing Customer

        /// <summary>
        /// Update Customer
        /// </summary>
        public object Put(CustomerRequest request)
        {
            var response = new CustomerDto();

            try
            {
                var token = Request.Headers.Get("securityToken");

                if (token == null)
                {
                    throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
                }

                var r = _authorizeService.AuthorizeCustomer(token);

                response = _customerService.ReturnCustomerByToken(token);

                if (response == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "User not found");
                }

                response.TriperooCustomers.Token = _authorizeService.AssignToken(request.Customer.Profile.EmailAddress, request.Customer.Profile.Pass);
                response.TriperooCustomers.Profile = request.Customer.Profile;
                response.TriperooCustomers.DateUpdate = DateTime.Now;
                response.TriperooCustomers.Profile.Pass = "";

                response = _customerService.InsertUpdateCustomer(response.TriperooCustomers.CustomerReference, response.TriperooCustomers);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        #endregion
    }

    #endregion
}
