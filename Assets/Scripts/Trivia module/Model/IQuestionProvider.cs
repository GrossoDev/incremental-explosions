using System.Threading.Tasks;

namespace Trivia.Model
{
    public interface IQuestionProvider
    {
        Task<TriviaQuestion> GetQuestionAsync(int difficulty);
    }
}
