using Microsoft.AspNetCore.Identity;
using SurveyBasket;
using SurveyBasket.Abstractions;

namespace Survey_Basket.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [HttpPost("")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
         
            var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
            return authResult.IsSuccess
                ? Ok(authResult.Value)
                : authResult.ToProblem(StatusCodes.Status400BadRequest);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            return authResult.IsSuccess
            ? Ok(authResult.Value)
            : authResult.ToProblem(StatusCodes.Status400BadRequest);
        }


        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem(statusCode: StatusCodes.Status400BadRequest);
        }
       

    }
}
