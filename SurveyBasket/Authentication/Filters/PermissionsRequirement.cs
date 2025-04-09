namespace SurveyBasket.Authentication.Filters
{
    public class PermissionsRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }
}
