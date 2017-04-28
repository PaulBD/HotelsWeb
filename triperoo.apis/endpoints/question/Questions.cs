using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace triperoo.apis.endpoints.review
{
    #region Customer Questions Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/location/{id}/questions", "GET")]
    public class QuestionsRequest
    {
        public int Id { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class QuestionsRequestValidator : AbstractValidator<QuestionsRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public QuestionsRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.Id).GreaterThan(0).WithMessage("Invalid location id have been supplied");
                RuleFor(r => r.PageSize).GreaterThan(0).WithMessage("Invalid page size has been supplied");
                RuleFor(r => r.PageNumber).GreaterThan(0).WithMessage("Invalid page number has been supplied");
            });
        }
    }

    #endregion

    #region API logic

    public class QuestionsApi : Service
    {
        private readonly IQuestionService _questionService;

        /// <summary>
        /// Constructor
        /// </summary>
        public QuestionsApi(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #region Return Questions By Id

        /// <summary>
        /// Return Questions By Id
        /// </summary>
        public object Get(QuestionsRequest request)
        {
            var response = new List<QuestionDto>();

            try
            {
                response = _questionService.ReturnQuestionsByLocationId(request.Id);

                if (request.PageNumber > 0)
                {
                    response = response.Skip(request.PageSize * request.PageNumber).Take(request.PageSize).ToList();
                }
                else
                {
                    response = response.Take(request.PageSize).ToList();
                }
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
