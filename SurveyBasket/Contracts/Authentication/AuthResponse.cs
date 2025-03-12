namespace Survey_Basket.Contracts.Authentication
{
    public record AuthResponse(string Id, string? Email, string FirstName, string LastName, string Token, int ExpiresIn,string RefreshToken, DateTime RefresTokenExpiration );
   
}
