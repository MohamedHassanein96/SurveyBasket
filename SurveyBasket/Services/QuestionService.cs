using Microsoft.Extensions.Caching.Hybrid;
using System.Linq.Dynamic.Core;

namespace SurveyBasket.Services
{
    public class QuestionService(ApplicationDbContext context, HybridCache hybridCache, ILogger<QuestionService> logger) : IQuestionService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly HybridCache _hybridCache = hybridCache;
        private readonly ILogger<QuestionService> _logger = logger;
        private const string _cachePrefix = "availableQuestions";

        public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default)
        {
            var question = await _context.Questions
                .Where(x => x.Id == id && x.PollId == pollId)
                .Include(x => x.Answers).Select(q => new QuestionResponse
                (q.Id,
                 q.Content,
                 q.Answers.Select(answer => new AnswerResponse(answer.Id, answer.Content)))).AsNoTracking().SingleOrDefaultAsync(cancellationToken);

            if (question is null)
                return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);

            return Result.Success(question);
        }

        public async Task<Result<PaginatedList<QuestionResponse>>> GetAllAsync(int pollId, RequestFilters filters, CancellationToken cancellationToken = default)
        {
            var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);

            if (!pollIsExists)
                return Result.Failure<PaginatedList<QuestionResponse>>(PollErrors.PollNotFound);

            var query = _context.Questions
                .Where(x => string.IsNullOrEmpty(filters.SearchValue) || x.Content.Contains(filters.SearchValue))
                .OrderBy(string.IsNullOrEmpty(filters.SortColumn) ? $"Id {filters.SortDirection}" : $"{filters.SortColumn} {filters.SortDirection}")
                .Include(x => x.Answers)
                .ProjectToType<QuestionResponse>()
                .AsNoTracking();

            var questions = await PaginatedList<QuestionResponse>.CreateAsync(query, filters.PageNumber, filters.PageSize, cancellationToken);

            return Result.Success(questions);
        }

        public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationToken);
            if (!pollIsExists)
                return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

            var questionIsExists = await _context.Questions.AnyAsync(x => x.Content == request.Content && x.PollId == pollId, cancellationToken: cancellationToken);
            if (questionIsExists)
                return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);

            var question = request.Adapt<Question>();
            question.PollId = pollId;

            await _context.AddAsync(question, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await _hybridCache.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);
            return Result.Success(question.Adapt<QuestionResponse>());
        }

        public async Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);
            if (question is null)
                return Result.Failure(QuestionErrors.QuestionNotFound);

            question.IsActive = !question.IsActive;

            await _context.SaveChangesAsync(cancellationToken);
            await _hybridCache.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var questionIsExistes = await _context.Questions.AnyAsync(
                x => x.PollId == pollId
                && x.Id != id
                && x.Content == request.Content
                , cancellationToken);

            if (questionIsExistes)
                return Result.Failure(QuestionErrors.DuplicatedQuestionContent);

            var question = await _context.Questions
                 .Include(x => x.Answers)
                 .SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);

            if (question is null)
                return Result.Failure(QuestionErrors.QuestionNotFound);

            question.Content = request.Content;

            var currentAnswers = question.Answers.Select(answer => answer.Content).ToList();

            var newAnswers = request.Answers.Except(currentAnswers).ToList();

            newAnswers.ForEach(answer =>
            question.Answers.Add(new Answer { Content = answer }));


            question.Answers.ToList().ForEach(answer =>
            {
                answer.IsActive = request.Answers.Contains(answer.Content);
            });

            await _context.SaveChangesAsync(cancellationToken);
            await _hybridCache.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

            return Result.Success();


        }

        public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default)
        {

            //var hasVote = await _context.Votes.AnyAsync(v => v.PollId == pollId && v.UserId == userId, cancellationToken);
            //if (hasVote)
            //    return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DuplicatedVote);


            //var pollIsExsits = await _context.Polls
            //     .SingleOrDefaultAsync(x => x.Id == pollId && x.IsPublished
            //     && DateOnly.FromDateTime(DateTime.UtcNow) >= x.StartsAt
            //     && DateOnly.FromDateTime(DateTime.UtcNow) <= x.EndsAt, cancellationToken);
            //if (pollIsExsits is null)
            //    return Result.Failure<IEnumerable<QuestionResponse>>(QuestionErrors.QuestionNotFound);

            var cahcheKey = $"{_cachePrefix}-{pollId}";

            var questions = await _hybridCache.GetOrCreateAsync<IEnumerable<QuestionResponse>>(
                cahcheKey, async cacheEntry => await _context.Questions
                .Where(q => q.PollId == pollId && q.IsActive)
                .Include(q => q.Answers)
                .Select(q => new QuestionResponse(
                    q.Id,
                    q.Content,
                    q.Answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.Id, a.Content))
               ))
                .AsNoTracking().ToListAsync(cancellationToken), cancellationToken: cancellationToken);

            return Result.Success<IEnumerable<QuestionResponse>>(questions);

        }


    }
}
