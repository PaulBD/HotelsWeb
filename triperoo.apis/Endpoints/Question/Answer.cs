using core.customers.dtos;
using core.customers.services;
using core.hotels.services;
using core.places.services;
using ServiceStack;
using System;
using System.Net;

namespace triperoo.apis.endpoints.review
{
    #region Answer Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/question/{questionId}/answer", "POST")]
    public class AnswerRequest
    {
        public string QuestionId { get; set; }
        public AnswerDto Answer { get; set; }
    }

    #endregion

    #region API logic

    public class AnswerApi : Service
    {
        private readonly IQuestionService _questionService;
        private readonly ICustomerService _customerService;
        private readonly ILocationService _locationService;

        /// <summary>
        /// Constructor
        /// </summary>
        public AnswerApi(IQuestionService questionService, ICustomerService customerService, ILocationService locationService)
        {
            _questionService = questionService;
            _customerService = customerService;
            _locationService = locationService;

        }

        #region Insert Answer

        /// <summary>
        /// Insert Answer
        /// </summary>
        public object Post(AnswerRequest request)
        {
            try
            {
                var token = Request.Headers["token"];

                if (string.IsNullOrEmpty(token))
                {
                    return new HttpResult("Token not found", HttpStatusCode.Unauthorized);
                }

                var customer = _customerService.ReturnCustomerByToken(token);

                if (customer == null)
                {
                    return new HttpResult("Customer not found" + token, HttpStatusCode.Unauthorized);
                }

                //var reference = "question:" + Guid.NewGuid();
                //request.Question.QuestionReference = reference;
                //request.Question.InventoryReference = request.Question.InventoryReference;
                //request.Question.DateCreated = DateTime.Now;
                //request.Question.CustomerReference = customer.TriperooCustomers.CustomerReference;

                _questionService.InsertNewAnswer(reference, request.Answer);
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
