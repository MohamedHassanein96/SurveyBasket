﻿
namespace Survey_Basket.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expireIn) GenerateToken(ApplicationUser user);
        string? ValidateToken(string token);
    }
}
