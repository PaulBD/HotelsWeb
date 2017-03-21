using System;
using core.customers.dtos;
using library.couchbase;
using System.Collections.Generic;

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
        /// Insert new review
        /// </summary>
        public void InsertNewQuestion(string reference, QuestionDetailDto review)
        {
            _couchbaseHelper.AddRecordToCouchbase(reference, review, _bucketName);
        }

        /// <summary>
        /// Process Query
        /// </summary>
        private List<QuestionDto> ProcessQuery(string q, int limit, int offset)
        {
            if (limit > 0)
            {
                q += " LIMIT " + limit + " OFFSET " + offset;
            }

            var result = _couchbaseHelper.ReturnQuery<QuestionDto>(q, _bucketName);

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Return Questions By Location Id
        /// </summary>
        public List<QuestionDto> ReturnQuestionsByLocationId(int id, int offset, int limit)
        {
            var q = "SELECT tr.question, tr.questionReference, tr.customerReference, tr.dateCreated, tr.inventoryReference, tr.isArchived, tr.type, tc.profile.name as customerName, tc.profile.imageUrl as customerImageUrl, tc.profile.profileUrl as customerProfileUrl FROM TriperooCustomers tr JOIN TriperooCustomers tc ON KEYS tr.customerReference";

            q += " WHERE tr.type = 'question' AND tr.inventoryReference = " + id + " ORDER BY tr.dateCreated DESC";

            return ProcessQuery(q, limit, offset);

        }
    }
}
