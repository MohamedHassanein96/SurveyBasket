namespace SurveyBasket.Erorrs
{
    public class UserErrors
    {
        public static readonly Error InvalidCredentials = new("User.InvalidCredentials", "Invalid Email Or Password", StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidJwtToken = new("User.nvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidRefreshToken = new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);
        public static readonly Error DuplicatedEmail = new("User.DuplicatedEmail", "there is another user with the same email", StatusCodes.Status409Conflict);
        public static readonly Error EmailNotConfirmed = new("User.Email is Not Confirmed", "Invalid refresh token", StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidCode = new("UserInvalidCode", "Invalid code", StatusCodes.Status401Unauthorized);
        public static readonly Error DuplicatedConfirmation = new("DuplicatedConfirmation", "Email Already Confirmed", StatusCodes.Status400BadRequest);

    }

}
