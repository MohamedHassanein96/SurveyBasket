
namespace SurveyBasket.Controllers
{
    [Route("api/polls/{pollId}/vote")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Member.Name)]
    [EnableRateLimiting("concurrency")]
    public class VotesController(IQuestionService questionService, IVoteService voteSerice) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;
        private readonly IVoteService _voteSerice = voteSerice;

        [HttpGet("")]
        public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await _questionService.GetAllAvailableAsync(pollId, userId!, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }
        [HttpPost("")]
        public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
        {
            var result = await _voteSerice.AddAsync(pollId, User.GetUserId()!, request, cancellationToken);

            return result.IsSuccess ? Created() : result.ToProblem();
        }
    }
}
