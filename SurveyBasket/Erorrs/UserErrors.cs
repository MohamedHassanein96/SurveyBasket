namespace SurveyBasket.Erorrs
{
    public class UserErrors
    {
        public static readonly Error InvalidCredentials = new("User.InvalidCredentials", "Invalid Email Or Password", StatusCodes.Status401Unauthorized);
        public static readonly Error DisabledUser = new("User.DisabledUser", "Disabled User,contact your administrator", StatusCodes.Status401Unauthorized);
        public static readonly Error LockedUser = new("User.LockedUser", "Locked User ,contact your administrator", StatusCodes.Status401Unauthorized);
        public static readonly Error UserNotFound = new("User.NotFound", "User is NotFound", StatusCodes.Status404NotFound);

        public static readonly Error InvalidJwtToken = new("User.nvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidRefreshToken = new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);
        public static readonly Error DuplicatedEmail = new("User.DuplicatedEmail", "there is another user with the same email", StatusCodes.Status409Conflict);
        public static readonly Error EmailNotConfirmed = new("User.Email is Not Confirmed", "Invalid refresh token", StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidCode = new("User.InvalidCode", "Invalid code", StatusCodes.Status401Unauthorized);
        public static readonly Error DuplicatedConfirmation = new("User.DuplicatedConfirmation", "Email Already Confirmed", StatusCodes.Status400BadRequest);
        public static readonly Error InvalidRoles = new("Role.InvalidRoles", "Invalid Roles", StatusCodes.Status400BadRequest);

    }

}
