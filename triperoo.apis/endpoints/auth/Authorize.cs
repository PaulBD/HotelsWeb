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
    [Route("/v1/authorize", "POST")]
    public class AuthorizeRequest
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class AuthorizeRequestValidator : AbstractValidator<AuthorizeRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AuthorizeRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.EmailAddress).NotNull().WithMessage("Supply a valid email address");
                RuleFor(r => r.Password).NotNull().WithMessage("Supply a valid password");
            });
        }
    }

    #endregion

    #region API logic

    public class AuthorizeApi : Service
    {
        private readonly ICustomerService _customerService;
        private readonly IAuthorizeService _authorizeService;

        /// <summary>
        /// Constructor
        /// </summary>
        public AuthorizeApi(ICustomerService customerService, IAuthorizeService authorizeService)
        {
            _customerService = customerService;
            _authorizeService = authorizeService;
        }

        #region Authorize Customer

        /// <summary>
        /// Authorize Customer
        /// </summary>
        public object Post(AuthorizeRequest request)
        {
            var authorizationDto = new AuthorizationDto();
            string token = null;

            try
            {
                token = _authorizeService.AssignToken(request.EmailAddress, request.Password);
                var response = _customerService.ReturnCustomerByToken(token);

                if (response == null)
				{
					throw HttpError.NotFound("Email Address or Password is invalid");
                }

                authorizationDto.Token = token;
                authorizationDto.UserImage = response.TriperooCustomers.Profile.ImageUrl;
                authorizationDto.UserName = response.TriperooCustomers.Profile.Name;
                authorizationDto.UserId = response.TriperooCustomers.CustomerReference.Replace("customer:", "");
                authorizationDto.BaseUrl = "/profile/" + response.TriperooCustomers.CustomerReference.Replace("customer:", "") + " / " + response.TriperooCustomers.Profile.Name.Replace(" ", "-");
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
