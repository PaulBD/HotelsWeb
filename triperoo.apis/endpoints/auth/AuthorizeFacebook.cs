using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.customers.services;
using core.customers.dtos;

namespace triperoo.apis.endpoints.auth
{
    #region Authorize a customer

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/authorize/facebook", "POST")]
    public class AuthorizeFacebookRequest
    {
        public string EmailAddress { get; set; }
        public int FacebookId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
		public string CurrentCity { get; set; }
		public int CurrentCityId { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class AuthorizeFacebookRequestValidator : AbstractValidator<AuthorizeFacebookRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AuthorizeFacebookRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Post, () =>
            {
                RuleFor(r => r.EmailAddress).NotNull().WithMessage("Supply a valid email address");
                RuleFor(r => r.FacebookId).NotNull().WithMessage("Supply a valid facebook id");
				RuleFor(r => r.Name).NotNull().WithMessage("Supply a valid name");
				RuleFor(r => r.CurrentCityId).GreaterThan(0).WithMessage("Supply a valid city");
                RuleFor(r => r.CurrentCity).NotNull().WithMessage("Supply a valid city");
            });
        }
    }

    #endregion

    #region API logic

    public class AuthorizeFacebookApi : Service
    {
        private readonly ICustomerService _customerService;
        private readonly IAuthorizeService _authorizeService;

        /// <summary>
        /// Constructor
        /// </summary>
        public AuthorizeFacebookApi(ICustomerService customerService, IAuthorizeService authorizeService)
        {
            _customerService = customerService;
            _authorizeService = authorizeService;
        }

        #region Authorize Facebook Customer

        /// <summary>
        /// Authorize Customer
        /// </summary>
        public object Post(AuthorizeFacebookRequest request)
        {
            CustomerDto response;
            string token = null;
            var authorizationDto = new AuthorizationDto();
            var customer = new CustomerDto();

            try
			{
				var guid = Guid.NewGuid().ToString().ToLower();

                //TODO: WHAT HAPPENS IF A FACEBOOK USER SWITCHES EMAIL ADDRESS??
                token = _authorizeService.AssignToken(request.EmailAddress, request.FacebookId.ToString());
                response = _customerService.ReturnCustomerByToken(token);

                if (response != null)
                {
                    customer = response;
                }
				else
				{
					customer.TriperooCustomers.DateCreated = DateTime.Now;
					customer.TriperooCustomers.CustomerReference = "customer:" + guid;
					customer.TriperooCustomers.Profile.ProfileUrl = "/profile/" + guid.ToLower() + "/" + request.Name.Replace(" ", "-").ToLower();
				}

				customer.TriperooCustomers.IsFacebookSignup = true;
				customer.TriperooCustomers.FacebookId = request.FacebookId;
                customer.TriperooCustomers.Profile.Name = request.Name;
				customer.TriperooCustomers.Profile.CurrentLocation = request.CurrentCity;
				customer.TriperooCustomers.Profile.CurrentLocationId = request.CurrentCityId;
                customer.TriperooCustomers.Profile.EmailAddress = request.EmailAddress;
                customer.TriperooCustomers.Token = token;
                customer.TriperooCustomers.Profile.ImageUrl = request.ImageUrl;
                customer.TriperooCustomers.LastLoginDate = DateTime.Now;

                _customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);

                authorizationDto.Token = token;
                authorizationDto.UserImage = customer.TriperooCustomers.Profile.ImageUrl;
				authorizationDto.UserName = customer.TriperooCustomers.Profile.Name;
				authorizationDto.UserId = customer.TriperooCustomers.CustomerReference.Replace("customer:", "");
                authorizationDto.BaseUrl = "/profile/" + customer.TriperooCustomers.CustomerReference.Replace("customer:", "") + "/" + request.Name.Replace(" ", "-").ToLower();
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(authorizationDto, HttpStatusCode.OK);
        }

        #endregion
    }

    #endregion
}
