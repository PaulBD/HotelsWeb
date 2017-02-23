using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Net;

namespace triperoo.apis.endpoints.customer
{
    #region Newsletter Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/customer/newsletter", "POST")]
    public class NewsletterRequest
    {
        public string EmailAddress { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class NewsletterRequestValidator : AbstractValidator<NewsletterRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NewsletterRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Post, () =>
            {
                RuleFor(r => r.EmailAddress).NotNull().WithMessage("Supply a valid email address");
            });
        }
    }

    #endregion

    #region API logic

    public class NewsletterApi : Service
    {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Constructor
        /// </summary>
        public NewsletterApi(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        #region Insert Newsletter

        /// <summary>
        /// Insert Customer
        /// </summary>
        public object Post(NewsletterRequest request)
        {
            var response = new NewsletterDto();

            try
            {
                response = _customerService.InsertNewsletter(new NewsletterDto() { EmailAddress = request.EmailAddress, DateAdded = DateTime.Now });
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
