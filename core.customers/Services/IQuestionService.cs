using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface IQuestionService
    {
		void InsertQuestion(string reference, QuestionDetailDto review);
        List<QuestionDto> ReturnQuestionsByLocationId(int id);
        QuestionDetailDto ReturnQuestionById(string reference);
    }
}
