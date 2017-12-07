using System;
using System.Data;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.customers.services;
using core.customers.dtos;
using library.common;

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
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ForgotPasswordRequestApi(ICustomerService customerService, IEmailService emailService)
        {
            _customerService = customerService;
            _emailService = emailService;
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

                _emailService.SendForgotPasswordReminder(response.TriperooCustomers.Profile.EmailAddress, response.TriperooCustomers.Profile.Name, response.TriperooCustomers.CustomerReference);
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

    #region Update customer password

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/update-password", "POST")]
    public class UpdatePasswordRequest
    {
        public string NewPassword { get; set; }
        public string EncryptedGuid { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class UpdatePasswordRequestValidator : AbstractValidator<UpdatePasswordRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpdatePasswordRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.NewPassword).NotNull().WithMessage("Supply a valid password");
                RuleFor(r => r.EncryptedGuid).NotNull().WithMessage("Supply a valid user id");
            });
        }
    }

    #endregion

    #region API logic

    public class UpdatePasswordRequestApi : Service
    {
        private readonly ICustomerService _customerService;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpdatePasswordRequestApi(ICustomerService customerService, IEmailService emailService)
        {
            _customerService = customerService;
            _emailService = emailService;
        }

        #region Update Password

        /// <summary>
        /// Update password
        /// </summary>
        public object Post(UpdatePasswordRequest request)
        {
            CustomerDto response;

            try
            {
                response = _customerService.ReturnCustomerByEncryptedGuid(System.Net.WebUtility.UrlDecode(request.EncryptedGuid));

                if (response == null)
                {
                    throw HttpError.NotFound("There has been a problem returning customer details");
                }

                response.TriperooCustomers.Profile.Pass = request.NewPassword;

                _customerService.InsertUpdateCustomer(response.TriperooCustomers.CustomerReference, response.TriperooCustomers);
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
