using core.customers.dtos;
using core.customers.services;
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
    [Route("/v1/question/answer", "POST")]
    public class AnswerRequest
    {
        public AnswerDto Answer { get; set; }
    }

	#endregion

	#region Like Answer Endpoint

	/// <summary>
	/// Request
	/// </summary>
    [Route("/v1/question/{questionReference}/answer/{answerReference}/like", "PUT")]
	public class LikeAnswerRequest
	{
		public string QuestionReference { get; set; }
		public string AnswerReference { get; set; }
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

                var question = _questionService.ReturnQuestionById(request.Answer.QuestionReference);

				var guid = Guid.NewGuid();

				var reference = "answer:" + guid;
                request.Answer.AnswerReference = reference;
                request.Answer.DateCreated = DateTime.Now;
				request.Answer.CustomerReference = customer.TriperooCustomers.CustomerReference;
				request.Answer.CustomerImageUrl = customer.TriperooCustomers.Profile.ImageUrl;
				request.Answer.CustomerName = customer.TriperooCustomers.Profile.Name;
				request.Answer.CustomerProfileUrl = customer.TriperooCustomers.Profile.ProfileUrl;

                question.Answers.Add(request.Answer);

                _questionService.InsertQuestion(question.QuestionReference, question);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(HttpStatusCode.OK);
        }

        #endregion

        #region Like Answer

        /// <summary>
        /// Like Answer
        /// </summary>
        public object Put(LikeAnswerRequest request)
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

                _questionService.LikeAnswer(request.QuestionReference, request.AnswerReference);
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
