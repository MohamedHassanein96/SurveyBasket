namespace SurveyBasket.Erorrs
{
    public class UserErrors
    {
        public static readonly Error InvalidCredentials = new("User.InvalidCredentials", "Invalid Email Or Password");
        public static readonly Error InvalidJwtToken = new("User.nvalidJwtToken", "Invalid Jwt token");
        public static readonly Error InvalidRefreshToken = new("User.InvalidRefreshToken", "Invalid refresh token");
    }

}
