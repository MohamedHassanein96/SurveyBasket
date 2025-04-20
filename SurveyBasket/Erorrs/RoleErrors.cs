namespace SurveyBasket.Erorrs
{
    public record RoleErrors
    {
        public static readonly Error RoleNotFound = new("Role.NotFound", "Role is Not Found", StatusCodes.Status404NotFound);
        public static readonly Error InvalidPermissions = new("Role.InvalidPermissions", "Invalid Permissions", StatusCodes.Status400BadRequest);
        public static readonly Error DuplicatedRole = new("Role.DuplicatedRole", "there is another Roel with the same Name", StatusCodes.Status409Conflict);

    }

}
