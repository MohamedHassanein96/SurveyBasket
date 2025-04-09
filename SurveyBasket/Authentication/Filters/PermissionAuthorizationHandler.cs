namespace SurveyBasket.Authentication.Filters
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionsRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirement requirement)
        {
            if (context.User.Identity is not { IsAuthenticated: true } ||
              !context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permissions.Type))
                return;

            context.Succeed(requirement);
            return;
        }
    }
}
