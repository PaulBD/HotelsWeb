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
    [Route("/v1/authorize")]
    public class AuthorizeRequest : Service
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
                RuleFor(r => r.EmailAddress).NotNull().WithMessage("Supply a valid username");
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

        #region Return Customer By Email & Password

        /// <summary>
        /// Lists location by reference (type:id)
        /// </summary>
        public object Get(AuthorizeRequest request)
        {
            CustomerDto response = new CustomerDto();

            try
            {
                _customerService.ReturnCustomerByUsernamePassword(request.EmailAddress, request.Password);
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
