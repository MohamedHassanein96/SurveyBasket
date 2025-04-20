namespace SurveyBasket.Contracts.Result
{
    public record PollVotesResponse(string Title, IEnumerable<VoteResponse> Votes);

}
