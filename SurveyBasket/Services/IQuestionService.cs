using SurveyBasket.Contracts.Common;

namespace SurveyBasket.Services
{
    public interface IQuestionService
    {
        public Task<Result<PaginatedList<QuestionResponse>>> GetAllAsync(int pollId, RequestFilters filters, CancellationToken cancellationToken =default);
        public Task<Result<IEnumerable<QuestionResponse>>> GetAllAvailableAsync(int pollId, string userId, CancellationToken cancellationToken =default);
        public Task<Result<QuestionResponse>> GetAsync(int pollId,int id, CancellationToken cancellationToken =default);
        public Task<Result<QuestionResponse>> AddAsync(int pollId,QuestionRequest request, CancellationToken cancellationToken =default);
        public Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken =default);
        public Task<Result> UpdateAsync(int pollId,int id, QuestionRequest request, CancellationToken cancellationToken =default);
    }
}
