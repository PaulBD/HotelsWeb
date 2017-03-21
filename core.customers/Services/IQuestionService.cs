using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface IQuestionService
    {
        void InsertNewQuestion(string reference, QuestionDetailDto review);
        List<QuestionDto> ReturnQuestionsByLocationId(int id, int offset, int limit);
    }
}
