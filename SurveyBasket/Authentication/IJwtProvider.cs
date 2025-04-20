namespace Survey_Basket.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expireIn) GenerateToken(ApplicationUser user, IEnumerable<string> Roles, IEnumerable<string> Permissions);
        string? ValidateToken(string token);
    }
}
