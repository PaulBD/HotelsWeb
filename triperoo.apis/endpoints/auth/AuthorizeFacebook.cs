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
        public string FacebookId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string CurrentCity { get; set; }
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
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.EmailAddress).NotNull().WithMessage("Supply a valid email address");
                RuleFor(r => r.FacebookId).NotNull().WithMessage("Supply a valid facebook id");
                RuleFor(r => r.Name).NotNull().WithMessage("Supply a valid name");
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

            try
            {
                token = _authorizeService.AssignToken(request.EmailAddress, request.FacebookId);
                response = _customerService.ReturnCustomerByToken(token);

                if (response == null)
                {
                    var customer = new Customer();
                    customer.DateCreated = DateTime.Now;
                    customer.IsFacebookSignup = true;
                    customer.Reference = "customer:" + Guid.NewGuid().ToString();
                    customer.Profile.Name = request.Name;
                    customer.Profile.CurrentCity = request.CurrentCity;
                    customer.Profile.EmailAddress = request.EmailAddress;
                    customer.Token = token;
                    customer.Profile.ImageUrl = request.ImageUrl;

                    _customerService.InsertUpdateCustomer(customer.Reference, customer);
                }

                authorizationDto.Token = token;
                authorizationDto.UserImage = response.TriperooCustomers.Profile.ImageUrl;
                authorizationDto.UserName = response.TriperooCustomers.Profile.Name;
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
