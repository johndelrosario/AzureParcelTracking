using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AzureParcelTracking.Application.Helpers.Interface;
using Microsoft.IdentityModel.Tokens;

namespace AzureParcelTracking.Application.Helpers.Implementation
{
    internal class JwtHelper : IJwtHelper
    {
        public JwtHelper()
        {
            Issuer = Environment.GetEnvironmentVariable("JwtIssuer");
            Key = Environment.GetEnvironmentVariable("JwtKey");
            Timeout = Convert.ToInt32(Environment.GetEnvironmentVariable("JwtTimeout"));
        }

        public string Issuer { get; }

        public string Key { get; }

        public int Timeout { get; }

        public string GetToken(Guid userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(userId))
            };

            var token = new JwtSecurityToken(
                Issuer,
                Issuer,
                claims,
                expires: DateTime.Now.AddMinutes(Timeout),
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal GetClaimsPrincipal(string bearerToken)
        {
            var validationParameter = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = Issuer,
                ValidIssuer = Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key))
            };

            ClaimsPrincipal result = null;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                result = handler.ValidateToken(bearerToken, validationParameter, out _);
            }
            catch
            {
                // ignored
            }

            return result;
        }
    }
}