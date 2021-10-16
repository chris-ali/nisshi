using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace Nisshi.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtIssuerOptions options;

        public JwtTokenGenerator(IOptions<JwtIssuerOptions> options)
        {
            this.options = options.Value;
        }

        public string CreateToken(string username, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, options.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, 
                    new DateTimeOffset(options.IssuedAt).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Role, role)
            };

            var jwt = new JwtSecurityToken(options.Issuer, options.Audience,
                claims, options.NotBefore, options.Expiration, options.SigningCredentials);
            
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}