﻿


namespace Survey_Basket.Authentication
{
    public class JwtProvider( IOptions<JwtOptions> options) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;

        public (string token, int expireIn) GenerateToken(ApplicationUser user)
        {
            Claim[] claims = [
                new(JwtRegisteredClaimNames.Sub,user.Id),
                new(JwtRegisteredClaimNames.Email,user.Email!),
                new(JwtRegisteredClaimNames.GivenName,user.FirstName),
                new(JwtRegisteredClaimNames.FamilyName,user.LastName),
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                ]; 

            var symmetricsSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key!));
            var signingCredentials = new SigningCredentials(symmetricsSecurityKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
                signingCredentials: signingCredentials
                );


      
            return (token: new JwtSecurityTokenHandler().WriteToken(token), expireIn: _options.ExpiryMinutes * 60);
        }

        public  string? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricsSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key!));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = symmetricsSecurityKey,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken );

                var jwtToken = (JwtSecurityToken)validatedToken;

                return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
