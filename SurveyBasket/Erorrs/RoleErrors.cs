namespace SurveyBasket.Erorrs
{
    public class RoleErrors
    {
        public static readonly Error RoleNotFound = new("Role.NotFound", "Role is Not Found", StatusCodes.Status404NotFound);
        public static readonly Error InvalidPermissions = new("Role.InvalidPermissions", "Invalid Permissions", StatusCodes.Status400BadRequest);
        //public static readonly Error InvalidRefreshToken = new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);
        public static readonly Error DuplicatedRole = new("Role.DuplicatedRole", "there is another Roel with the same Name", StatusCodes.Status409Conflict);
        //public static readonly Error EmailNotConfirmed = new("User.Email is Not Confirmed", "Invalid refresh token", StatusCodes.Status401Unauthorized);
        //public static readonly Error InvalidCode = new("UserInvalidCode", "Invalid code", StatusCodes.Status401Unauthorized);
        //public static readonly Error DuplicatedConfirmation = new("DuplicatedConfirmation", "Email Already Confirmed", StatusCodes.Status400BadRequest);

    }

}
