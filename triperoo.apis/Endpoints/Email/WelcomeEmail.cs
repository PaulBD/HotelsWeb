using ServiceStack;
using System;
using System.Net;
using library.common;

namespace triperoo.apis.endpoints.email
{
    #region Welcome Email Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/emails/welcomeemail", "GET")]
    public class WelcomeEmailRequest
    {
        public string emailAddress { get; set; }
        public string name { get; set; }
        public string town { get; set; }
    }

    #endregion


    #region API logic

    public class WelcomeEmailApi : Service
    {
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor
        /// </summary>
        public WelcomeEmailApi(IEmailService emailService)
        {
            _emailService = emailService;

        }

        #region Send Welcome Email

        /// <summary>
        /// Send Welcome Email
        /// </summary>
        public object Get(WelcomeEmailRequest request)
        {
            try
            {
                _emailService.SendWelcomeEmail(request.emailAddress, request.name, request.town);
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
