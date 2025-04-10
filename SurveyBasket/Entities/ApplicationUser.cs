namespace Survey_Basket.Entities
{
    public class ApplicationUser :IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsDisabled { get; set; } =false;
        public List<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
