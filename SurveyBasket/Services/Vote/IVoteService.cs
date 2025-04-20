namespace SurveyBasket.Services.Vote
{
    public interface IVoteService
    {
        Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken = default);
    }
}
