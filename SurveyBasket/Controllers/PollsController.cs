﻿namespace Survey_Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PollsController(IPollService pollService) : ControllerBase
    {
        private readonly IPollService _pollService = pollService;

        

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls =await _pollService.GetAllAsync(cancellationToken);
            var response = polls.Adapt<IEnumerable<PollResponse>>();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id ,CancellationToken cancellationToken)
        {
           var result = await _pollService.GetAsync(id, cancellationToken);
            return result.IsSuccess 
                ? Ok(result.Value) 
                : result.ToProblem(statusCode: StatusCodes.Status404NotFound);
          
        }
      
        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] PollRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _pollService.AddAsync(request, cancellationToken);

            return result.IsSuccess 
                ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value) 
                : result.ToProblem(statusCode: StatusCodes.Status409Conflict);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
        {
            var result = await _pollService.UpdateAsync(id, request, cancellationToken);
            if (result.IsSuccess)
                return NoContent();

            else if (result.IsFailure && result.Error.Code== "Poll.DuplicatedTitle")
            return result.ToProblem(statusCode: StatusCodes.Status409Conflict); 

            else
             return result.ToProblem(statusCode: StatusCodes.Status404NotFound);
 
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.DeleteAsync(id, cancellationToken);

            return result.IsSuccess 
                ? NoContent()
                :result.ToProblem(statusCode: StatusCodes.Status404NotFound);
        }


        [HttpPut("{id}/TogglePublish")]
        public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.TogglePublishStatusAsync(id, cancellationToken);
            return result.IsSuccess 
                ? NoContent() 
                : result.ToProblem(statusCode: StatusCodes.Status404NotFound);
        }
    }
}
