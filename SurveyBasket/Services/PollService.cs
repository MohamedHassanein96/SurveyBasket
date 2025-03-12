


namespace Survey_Basket_Test.Services
{
    public class PollService(ApplicationDbContext context) : IPollService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default) => await _context.polls.AsNoTracking().ToListAsync(cancellationToken);


        public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _context.polls.FindAsync(id, cancellationToken);

            return poll is not null 
                 ? Result.Success(poll.Adapt<PollResponse>())
                 : Result.Failure<PollResponse>(PollErrors.PollNotFound);

        }
        public async Task<PollResponse> AddAsync(PollRequest request, CancellationToken cancellationToken = default )
        {
            var poll = request.Adapt<Poll>();
            await _context.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return poll.Adapt<PollResponse>();
        }


        public async Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken = default)
        {
            var currentPoll = await _context.polls.FindAsync(id, cancellationToken);
            if (currentPoll is null)
                return Result.Failure(PollErrors.PollNotFound);


            currentPoll.Title    =  request.Title;
            currentPoll.Summary  =  request.Summary;
            currentPoll.StartsAt =  request.StartsAt;
            currentPoll.EndsAt   =  request.EndsAt;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }


        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var poll = await _context.polls.FindAsync(id,cancellationToken);
            if (poll is null)
                return Result.Failure(PollErrors.PollNotFound);
            _context.Remove(poll);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(); 
        }

        

        public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken)
        {
            var poll = await _context.polls.FindAsync(id, cancellationToken);
            if (poll is null)
                return Result.Failure(PollErrors.PollNotFound);
            poll.IsPublished = !poll.IsPublished;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

       
    }
}
