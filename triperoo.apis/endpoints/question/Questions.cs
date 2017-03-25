using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;

namespace triperoo.apis.endpoints.review
{
    #region Customer Questions Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/questions", "GET")]
    public class QuestionsRequest
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int LocationId { get; set; }
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
                response = _questionService.ReturnQuestionsByLocationId(request.LocationId, request.Offset, request.Limit);
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
