using System;
using System.Data;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.customers.services;
using core.customers.dtos;

namespace triperoo.apis.endpoints.auth
{
    #region Send customer password

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/reset-password", "POST")]
    public class ForgotPasswordRequest
    {
        public string EmailAddress { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ForgotPasswordRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.EmailAddress).NotNull().WithMessage("Supply a valid email address");
            });
        }
    }

    #endregion

    #region API logic

    public class ForgotPasswordRequestApi : Service
    {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ForgotPasswordRequestApi(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        #region Send forgot password to Customer

        /// <summary>
        /// Send forgot password to Customer
        /// </summary>
        public object Post(ForgotPasswordRequest request)
        {
            CustomerDto response;

            try
            {
                response = _customerService.ReturnCustomerByEmailAddress(request.EmailAddress);

                if (response == null)
                {
                    throw HttpError.NotFound("Email Address not found");
                }

                //TODO: Send password reset
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(HttpStatusCode.OK);
        }

        #endregion
    }

    #endregion
}
