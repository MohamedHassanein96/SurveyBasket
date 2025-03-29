using SurveyBasket.Contracts.Result;

namespace SurveyBasket.Contracts.Vote
{
    public record VotesPerQuestionResponse(string Content , IEnumerable<VotesPerAnswerResponse> VotesPerAnswerResponses);
    
}
