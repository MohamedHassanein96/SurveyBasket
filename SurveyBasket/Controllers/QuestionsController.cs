namespace SurveyBasket.Controllers
{
    [Route("/api/polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController(IQuestionService questionService, ApplicationDbContext context) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;
        private readonly ApplicationDbContext _context = context;

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _questionService.GetAllAsync(pollId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int pollId, [FromRoute] int id , CancellationToken cancellationToken)
        {
            var result = await _questionService.GetAsync(pollId , id ,cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }
        [HttpPost("")]
        public async Task<IActionResult> AddAsync([FromRoute] int pollId, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
        {
            var result = await _questionService.AddAsync(pollId, request, cancellationToken);
   

                return result.IsSuccess
                    ? CreatedAtAction(nameof(Get), new { pollId, result.Value.Id }, result.Value)
                    : result.ToProblem();
         
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
        {
            var result = await _questionService.UpdateAsync(pollId, id, request, cancellationToken);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
         
        }

        [HttpPut("{id}/toggleStatus")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _questionService.ToggleStatusAsync(pollId, id, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    } 
}
