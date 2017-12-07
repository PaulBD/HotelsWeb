using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.customers.services;
using core.customers.dtos;
using library.common;

namespace triperoo.apis.endpoints.auth
{
    #region Authorize a customer

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/authorize/facebook", "GET")]
    [Route("/v1/authorize/facebook", "POST")]
    public class AuthorizeFacebookRequest
    {
        public string AccessToken { get; set; }
        public string EmailAddress { get; set; }
        public int FacebookId { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
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
            });
        }
    }

    #endregion

    #region API logic

    public class AuthorizeFacebookApi : Service
    {
        private readonly ICustomerService _customerService;
        private readonly IAuthorizeService _authorizeService;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor
        /// </summary>
        public AuthorizeFacebookApi(ICustomerService customerService, IAuthorizeService authorizeService, IEmailService emailService)
        {
            _customerService = customerService;
            _authorizeService = authorizeService;
            _emailService = emailService;
        }

        #region Get Facebook Customer

        /// <summary>
        /// Get Customer
        /// </summary>
        public object Get(AuthorizeFacebookRequest request)
        {
            CustomerDto response = new CustomerDto();

            try
            {
                var token = _authorizeService.AssignToken(request.EmailAddress, request.FacebookId.ToString());
                response = _customerService.ReturnCustomerByToken(token);

            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        #endregion

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
            bool isNewSignup = false;

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
                    isNewSignup = true;
					customer.TriperooCustomers.DateCreated = DateTime.Now;
					customer.TriperooCustomers.CustomerReference = guid;

                    var url = "/profile/" + guid.ToLower() + "/" + request.Name.Replace(" ", "-");
                    customer.TriperooCustomers.Profile.ProfileUrl = url.ToLower();;
				}

				customer.TriperooCustomers.IsFacebookSignup = true;
                customer.TriperooCustomers.FacebookId = request.FacebookId;
                customer.TriperooCustomers.AccessToken = request.AccessToken;
                customer.TriperooCustomers.Profile.Name = request.Name;

                if (!string.IsNullOrEmpty(request.CurrentCity))
                {
                    customer.TriperooCustomers.Profile.CurrentLocation = request.CurrentCity;
                    customer.TriperooCustomers.Profile.CurrentLocationId = request.CurrentCityId;
                }

                customer.TriperooCustomers.Profile.EmailAddress = request.EmailAddress;
                customer.TriperooCustomers.Token = token;
                customer.TriperooCustomers.Profile.ImageUrl = request.ImageUrl;
                customer.TriperooCustomers.LastLoginDate = DateTime.Now;

                _customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);

                authorizationDto.Token = token;
                authorizationDto.CurrentLocationId = customer.TriperooCustomers.Profile.CurrentLocationId;
                authorizationDto.UserImage = customer.TriperooCustomers.Profile.ImageUrl;
				authorizationDto.UserName = customer.TriperooCustomers.Profile.Name;
				authorizationDto.UserId = customer.TriperooCustomers.CustomerReference;
                var baseUrl = "/profile/" + customer.TriperooCustomers.CustomerReference + "/" + request.Name.Replace(" ", "-");
                authorizationDto.BaseUrl = baseUrl.ToLower();;
                authorizationDto.IsNewSignup = isNewSignup;

                if (isNewSignup)
                {
                    _emailService.SendWelcomeEmail(request.EmailAddress, request.Name, request.CurrentCity);
                }
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
