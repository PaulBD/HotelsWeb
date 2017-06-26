using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface IQuestionService
    {
		void InsertQuestion(string reference, QuestionDetailDto review);
        List<QuestionDto> ReturnQuestionsByLocationId(int id);
        QuestionDto ReturnFullQuestionById(string reference);
        QuestionDetailDto ReturnQuestionById(string reference);
        void LikeQuestion(string questionReference);
        void LikeAnswer(string questionReference, string answerReference);
    }
}
