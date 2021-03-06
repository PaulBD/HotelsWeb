﻿using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.customers.services;
using core.customers.dtos;
using library.common;

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
        public string Name { get; set; }
		public string CurrentCity { get; set; }
        public int CurrentCityId { get; set; }
        public bool OptIn { get; set; }
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
                RuleFor(r => r.Name).NotNull().WithMessage("Supply a valid name");
                RuleFor(r => r.CurrentCity).NotNull().WithMessage("Supply a valid city");
            });
        }
    }

    #endregion

    #region API logic

    public class RegisterApi : Service
    {
        private readonly ICustomerService _customerService;
        private readonly IEmailService _emailService;
        private readonly IAuthorizeService _authorizeService;

        /// <summary>
        /// Constructor
        /// </summary>
        public RegisterApi(ICustomerService customerService, IAuthorizeService authorizeService, IEmailService emailService)
        {
            _customerService = customerService;
            _authorizeService = authorizeService;
            _emailService = emailService;
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
                response = _customerService.ReturnCustomerByEmailAddress(request.EmailAddress);

                if (response != null)
                {
					throw HttpError.Conflict("You are already a member of Triperoo");
                }

				var guid = Guid.NewGuid().ToString().ToLower();
				token = _authorizeService.AssignToken(request.EmailAddress, request.Password);

                var customer = new Customer();
                customer.OptIn = request.OptIn;     
                customer.DateCreated = DateTime.Now;
                customer.IsFacebookSignup = false;
                customer.CustomerReference = guid;
                customer.Profile.Name = request.Name;
				customer.Profile.CurrentLocationId = request.CurrentCityId; //TODO: Fix this
				customer.Profile.CurrentLocation = request.CurrentCity; //TODO: Fix this
				customer.Profile.EmailAddress = request.EmailAddress;
                customer.Profile.Pass = request.Password;
                customer.Token = token;
                customer.LastLoginDate = DateTime.Now;

                var url = "/profile/" + guid.ToLower() + "/" + customer.Profile.Name.Replace(" ", "-");
                customer.Profile.ProfileUrl = url.ToLower();;

                _customerService.InsertUpdateCustomer(customer.CustomerReference, customer);

                authorizationDto.Token = token;
                authorizationDto.IsNewSignup = true;
                authorizationDto.CurrentLocationId = request.CurrentCityId;
                authorizationDto.UserImage = "";
				authorizationDto.UserName = customer.Profile.Name;
                authorizationDto.UserId = customer.CustomerReference;
                authorizationDto.BaseUrl = url.ToLower();

                _emailService.SendWelcomeEmail(request.EmailAddress, request.Name, request.CurrentCity);

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
