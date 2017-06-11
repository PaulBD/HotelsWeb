using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Net;

namespace triperoo.apis.endpoints.customer
{
    #region Authorized Customer Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/customer", "GET")]
    [Route("/v1/customer", "PUT")]
    [Route("/v1/customer", "POST")]
    public class AuthorizedCustomerRequest
    {
        public ProfileDto Profile { get; set; }
    }

	#endregion

	#region Customer Endpoint

	/// <summary>
	/// Request
	/// </summary>
    [Route("/v1/customer/{customerReference}", "GET")]
	public class CustomerRequest
	{
		public string CustomerReference { get; set; }
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
				response = _customerService.ReturnCustomerByReference(request.CustomerReference);

				if (response == null)
				{
					throw new HttpError(HttpStatusCode.BadRequest, "Bad Request");
				}

				response.TriperooCustomers.Token = "";
				response.TriperooCustomers.Profile.Pass = "";
			}
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(response, HttpStatusCode.OK);
		}

		#endregion

		#region Return Authroized Customer By Reference

		/// <summary>
		/// Return Customer
		/// </summary>
		public object Get(AuthorizedCustomerRequest request)
        {
            CustomerDto response = new CustomerDto();

            try
            {
                var token = Request.Headers.Get("token");

                if (token == null)
                {
                    throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
                }

                response = _customerService.ReturnCustomerByToken(token);

                if (response == null)
                {
                    throw new HttpError(HttpStatusCode.BadRequest, "Bad Request");
				}

				response.TriperooCustomers.Token = "";
				response.TriperooCustomers.Profile.Pass = "";
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
        public object Put(AuthorizedCustomerRequest request)
        {
            var response = new CustomerDto();

            try
            {
                var token = Request.Headers.Get("token");

                if (token == null)
                {
					throw HttpError.Unauthorized("You are unauthorized to access this page");
                }

                var r = _authorizeService.AuthorizeCustomer(token);

				if (r == null)
				{
					throw HttpError.NotFound("Customer details cannot be found");
				}

                response = _customerService.ReturnCustomerByEmailAddress(r.EmailAddress);

                if (response == null)
				{
					throw HttpError.NotFound("Customer details cannot be found");
                }

                if (response.TriperooCustomers.IsFacebookSignup)
                {
					response.TriperooCustomers.Token = _authorizeService.AssignToken(request.Profile.EmailAddress, response.TriperooCustomers.FacebookId.ToString());
				}
                else
                {
                    response.TriperooCustomers.Token = _authorizeService.AssignToken(request.Profile.EmailAddress, response.TriperooCustomers.Profile.Pass);
                }
                response.TriperooCustomers.Profile.Name = request.Profile.Name;
                response.TriperooCustomers.Profile.DateOfBirth = request.Profile.DateOfBirth;
                response.TriperooCustomers.Profile.CurrentLocation = request.Profile.CurrentLocation;
                response.TriperooCustomers.Profile.CurrentLocationId = request.Profile.CurrentLocationId;
				response.TriperooCustomers.Profile.Pass = request.Profile.Pass;
				response.TriperooCustomers.Profile.Bio = request.Profile.Bio;
				response.TriperooCustomers.Profile.PhoneNumber = request.Profile.PhoneNumber;
                response.TriperooCustomers.DateUpdate = DateTime.Now;

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
