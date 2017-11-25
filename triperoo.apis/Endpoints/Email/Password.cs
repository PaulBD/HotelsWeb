using ServiceStack;
using System;
using System.Net;
using library.common;

namespace triperoo.apis.endpoints.email
{
    #region Forgot Password Reminder Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/emails/forgotPasswordReminder", "GET")]
    public class ForgotPasswordReminderRequest
    {
        public string emailAddress { get; set; }
        public string name { get; set; }
    }

    #endregion


    #region Forgot Password Confirmation Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/emails/forgotPasswordConfirmation", "GET")]
    public class ForgotPasswordConfirmationRequest
    {
        public string emailAddress { get; set; }
        public string name { get; set; }
    }

    #endregion


    #region API logic

    public class ForgotPasswordApi : Service
    {
        private readonly IEmail _emailService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ForgotPasswordApi(IEmail emailService)
        {
            _emailService = emailService;

        }

        #region Forgot Password Reminder Email

        /// <summary>
        /// Forgot Password Reminder
        /// </summary>
        public object Get(ForgotPasswordReminderRequest request)
        {
            try
            {
                _emailService.SendForgotPasswordReminder(request.emailAddress, request.name, "http://www.google.com");
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(HttpStatusCode.OK);
        }

        #endregion

        #region Forgot Password Confirmation Email

        /// <summary>
        /// Forgot Password Confirmation
        /// </summary>
        public object Get(ForgotPasswordConfirmationRequest request)
        {
            try
            {
                _emailService.SendForgotPasswordConfirmation(request.emailAddress, request.name);
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
