using System;
using core.customers.dtos;
using library.couchbase;
using System.Collections.Generic;
using System.Linq;

namespace core.customers.services
{
    public class QuestionService : IQuestionService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooCustomers";

        public QuestionService()
        {
            _couchbaseHelper = new CouchBaseHelper();
        }

        /// <summary>
        /// Insert new question
        /// </summary>
        public void InsertQuestion(string reference, QuestionDetailDto review)
        {
            _couchbaseHelper.AddRecordToCouchbase(reference, review, _bucketName);
		}

		/// <summary>
		/// Process Query
		/// </summary>
		private List<QuestionDto> ProcessQuery(string q)
        {
            var result = _couchbaseHelper.ReturnQuery<QuestionDto>(q, _bucketName);

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// HACK: Make this generic
        /// </summary>
        /// <returns>The detail query.</returns>
        /// <param name="q">Q.</param>
        private List<QuestionDetailDto> ProcessDetailQuery(string q)
		{
			var result = _couchbaseHelper.ReturnQuery<QuestionDetailDto>(q, _bucketName);

			if (result.Count > 0)
			{
				return result;
			}

			return null;
		}

        /// <summary>
        /// Return Questions By Location Id
        /// </summary>
        public List<QuestionDto> ReturnQuestionsByLocationId(int id)
        {
            var q = "SELECT tr.question, tr.questionReference, tr.questionUrl, tr.likeCount, tr.customerReference, tr.dateCreated, tr.inventoryReference, tr.isArchived, tr.type, tr.answers, tc.profile.name as customerName, tc.profile.imageUrl as customerImageUrl, tc.profile.profileUrl as customerProfileUrl FROM TriperooCustomers tr JOIN TriperooCustomers tc ON KEYS tr.customerReference";

            q += " WHERE tr.type = 'question' AND tr.inventoryReference = " + id + " ORDER BY tr.dateCreated DESC";

            return ProcessQuery(q);
		}

        /// <summary>
        /// Return Question By Question Reference
        /// </summary>
        public QuestionDto ReturnFullQuestionById(string reference)
        {
            var q = "SELECT tr.question, tr.questionReference, tr.questionUrl,  tr.likeCount, tr.customerReference, tr.dateCreated, tr.inventoryReference, tr.isArchived, tr.type, tr.answers, tc.profile.name as customerName, tc.profile.imageUrl as customerImageUrl, tc.profile.profileUrl as customerProfileUrl FROM TriperooCustomers tr JOIN TriperooCustomers tc ON KEYS tr.customerReference";

            q += " WHERE tr.type = 'question' AND tr.questionReference = '" + reference + "' ORDER BY tr.dateCreated DESC";

            var questionList = ProcessQuery(q);

            if (questionList.Count > 0)
            {
                return questionList[0];
            }

            return null;
        }

		/// <summary>
		/// Return Question By Question Reference
		/// </summary>
		public QuestionDetailDto ReturnQuestionById(string reference)
		{
			var q = "SELECT customerReference, dateCreated, inventoryReference, likeCount, isArchived, question, questionReference, type, answers, questionUrl FROM TriperooCustomers";

			q += " WHERE type = 'question' AND questionReference = '" + reference + "' ORDER BY dateCreated DESC";

			var questionList = ProcessDetailQuery(q);

            if (questionList.Count > 0)
            {
                return questionList[0];
            }

            return null;
		}

        /// <summary>
        /// Likes the question.
        /// </summary>
		public void LikeQuestion(string questionReference)
		{
			if (!questionReference.Contains("question:"))
			{
				questionReference = "question:" + questionReference;
			}

			var q = "UPDATE " + _bucketName + " SET likeCount = likeCount + 1 WHERE questionReference = '" + questionReference + "'";

			ProcessQuery(q);
		}

        /// <summary>
        /// Likes the answer.
        /// </summary>
		public void LikeAnswer(string questionReference, string answerReference)
		{
            if (!questionReference.Contains("question:"))
            {
                questionReference = "question:" + questionReference;
			}

			if (!answerReference.Contains("answer:"))
			{
				answerReference = "answer:" + answerReference;
			}
			
            var response = ReturnQuestionById(questionReference);

            if (response != null)
            {
                if (response.Answers.Count > 0)
                {
                    response.Answers.FirstOrDefault(q => q.AnswerReference == answerReference).LikeCount += 1;
                    InsertQuestion(response.QuestionReference, response);

                }
            }
		}
    }
}
