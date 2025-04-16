using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using SurveyBasket.Authentication.Filters;

namespace Survey_Basket.Controllers
{

    [ApiVersion(1, Deprecated = true)]
    [ApiVersion(2)] 
    [Route("api/[controller]")]
    [ApiController]
  



    public class PollsController(IPollService pollService) : ControllerBase
    {
        private readonly IPollService _pollService = pollService;
        [EnableRateLimiting("userLimit")]
        [HttpGet("")]
        [HasPermission(Permissions.ReadPolls)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls =await _pollService.GetAllAsync(cancellationToken);
            var response = polls.Adapt<IEnumerable<PollResponse>>();
            return Ok(response);
        }


        [MapToApiVersion(1)]
        [HttpGet("current")]
        [Authorize(Roles = DefaultRoles.Member)]
        public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
        {
            return Ok(await _pollService.GetCurrentAsyncV1(cancellationToken));
        }


        [MapToApiVersion(2)]
        [HttpGet("current")]
        [Authorize(Roles = DefaultRoles.Member)]
        public async Task<IActionResult> GetCurrentV2(CancellationToken cancellationToken)
        {
            return Ok(await _pollService.GetCurrentAsyncV2(cancellationToken));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id ,CancellationToken cancellationToken)
        {
           var result = await _pollService.GetAsync(id, cancellationToken);
            return result.IsSuccess 
                ? Ok(result.Value) 
                : result.ToProblem();
          
        }
      


        [HttpPost("")]
        [HasPermission(Permissions.AddPolls)]
        public async Task<IActionResult> Add([FromBody] PollRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _pollService.AddAsync(request, cancellationToken);

            return result.IsSuccess 
                ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value) 
                : result.ToProblem();
        }


        [HttpPut("{id}")]
        [HasPermission(Permissions.UpdatePolls)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
        {
            var result = await _pollService.UpdateAsync(id, request, cancellationToken);
            if (result.IsSuccess)
                return NoContent();

            else if (result.IsFailure && result.Error.Code== "Poll.DuplicatedTitle")
            return result.ToProblem(); 

            else
             return result.ToProblem();
        }


        [HttpDelete("{id}")]
        [HasPermission(Permissions.DeletePolls)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.DeleteAsync(id, cancellationToken);

            return result.IsSuccess 
                ? NoContent()
                :result.ToProblem();
        }


        [HttpPut("{id}/TogglePublish")]
        [HasPermission(Permissions.UpdatePolls)]
        public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.TogglePublishStatusAsync(id, cancellationToken);
            return result.IsSuccess 
                ? NoContent() 
                : result.ToProblem();
        }
       

    }
}
