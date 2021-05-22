using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace BLL.Services
{
    public static class JWT
    {
        public static string GenerateToken(BLL.Models.User user)
        {
            var clamis = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, user.Id),
                new System.Security.Claims.Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.UserName),
                new System.Security.Claims.Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new System.Security.Claims.Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.GivenName, user.FirstName),
                new System.Security.Claims.Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber)
            };

            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes("SOME_RANDOM_KEY_DO_NOT_SHARE"));
            var signature = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: "http://hucoin.com",
                audience: "http://hucoin.com",
                claims: clamis,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: signature);
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }
        public static BLL.Models.User GetUser(string Token)
        {
            if (string.IsNullOrWhiteSpace(Token)) return null;
            try
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(Token);

                return new Models.User
                {
                    Id = jwtSecurityToken.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value,
                    UserName = jwtSecurityToken.Claims.SingleOrDefault(claim => claim.Type == Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub).Value,
                    PhoneNumber = jwtSecurityToken.Claims.SingleOrDefault(claim => claim.Type == Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.PhoneNumber).Value,
                    FirstName = jwtSecurityToken.Claims.SingleOrDefault(claim => claim.Type == Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.GivenName).Value,
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
