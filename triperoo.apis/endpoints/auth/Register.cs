using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.customers.services;
using core.customers.dtos;

namespace triperoo.apis.endpoints.auth
{
    #region Register a new customer

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/register", "POST")]
    public class RegisterRequest
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CurrentCity { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RegisterRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.EmailAddress).NotNull().WithMessage("Supply a valid email address");
                RuleFor(r => r.Password).NotNull().WithMessage("Supply a valid password");
                RuleFor(r => r.FirstName).NotNull().WithMessage("Supply a valid first name");
                RuleFor(r => r.LastName).NotNull().WithMessage("Supply a valid last name");
                RuleFor(r => r.CurrentCity).NotNull().WithMessage("Supply a valid city");
            });
        }
    }

    #endregion

    #region API logic

    public class RegisterApi : Service
    {
        private readonly ICustomerService _customerService;
        private readonly IAuthorizeService _authorizeService;

        /// <summary>
        /// Constructor
        /// </summary>
        public RegisterApi(ICustomerService customerService, IAuthorizeService authorizeService)
        {
            _customerService = customerService;
            _authorizeService = authorizeService;
        }

        #region Register Customer

        /// <summary>
        /// Authorize Customer
        /// </summary>
        public object Post(RegisterRequest request)
        {
            CustomerDto response;
            string token = null;
            var authorizationDto = new AuthorizationDto();

            try
            {
                token = _authorizeService.AssignToken(request.EmailAddress, request.Password);
                response = _customerService.ReturnCustomerByToken(token);

                if (response != null)
                {
                    return new HttpResult("You are already a member of Triperoo", HttpStatusCode.Conflict);
                }

                var customer = new Customer();
                customer.DateCreated = DateTime.Now;
                customer.IsFacebookSignup = false;
                customer.Reference = "customer:" + Guid.NewGuid().ToString();
                customer.Profile.Name = request.FirstName + " " + request.LastName;
                customer.Profile.FirstName = request.FirstName;
                customer.Profile.LastName = request.LastName;
                customer.Profile.CurrentCity = request.CurrentCity;
                customer.Profile.EmailAddress = request.EmailAddress;
                customer.Profile.Pass = request.Password;
                customer.Token = token;

                _customerService.InsertUpdateCustomer(customer.Reference, customer);

                authorizationDto.Token = token;
                authorizationDto.UserImage = "";
                authorizationDto.UserName = customer.Profile.Name;
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
