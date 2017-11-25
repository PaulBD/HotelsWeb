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

        /// <summary>
        /// Constructor
        /// </summary>
        public UpdatePasswordRequestApi(ICustomerService customerService)
        {
            _customerService = customerService;
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
                response = _customerService.ReturnCustomerByEncryptedGuid(request.EncryptedGuid);

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
