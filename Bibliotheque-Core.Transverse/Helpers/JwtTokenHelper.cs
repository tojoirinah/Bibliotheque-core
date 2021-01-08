using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace Bibliotheque.Transverse.Helpers
{
    public class JwtTokenHelper
    {
        public static string CreateToken(ClaimsIdentity claimsIdentity, int expiresMinute, string secretKey)
        {
            DateTime issuedAt = DateTime.UtcNow;
            DateTime notBefore = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = issuedAt.AddMinutes(expiresMinute);

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                Expires = expires,
                IssuedAt = issuedAt,
                NotBefore = notBefore,
                SigningCredentials = signingCredentials
            };

            //create the jwt
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public IEnumerable<Claim> GetTokenClaims(string token, string secretKey)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("token is null or empty");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters(secretKey);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                SecurityToken validatedToken;
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                return tokenValid.Claims;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        bool IsTokenValid(string token, string secretKey)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("token is null or empty");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters(secretKey);

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                SecurityToken validatedToken = null;
                jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                return validatedToken != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        TokenValidationParameters GetTokenValidationParameters(string secretKey)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey))
            };

            return validationParameters;
        }
    }
}
