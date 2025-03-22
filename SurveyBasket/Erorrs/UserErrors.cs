namespace SurveyBasket.Erorrs
{
    public class UserErrors
    {
        public static readonly Error InvalidCredentials = new("User.InvalidCredentials", "Invalid Email Or Password", StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidJwtToken = new("User.nvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidRefreshToken = new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);
    }

}
