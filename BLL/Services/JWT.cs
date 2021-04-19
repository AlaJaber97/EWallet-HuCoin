﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public static class JWT
    {
        public static string GenerateToken(BLL.Models.User user)
        {
            var clamis = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.UserName),
                new System.Security.Claims.Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new System.Security.Claims.Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.GivenName, user.FirstName)
            };

            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes("https://HuCoin.com"));
            var signature = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                claims: clamis,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: signature);
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }
    }
}