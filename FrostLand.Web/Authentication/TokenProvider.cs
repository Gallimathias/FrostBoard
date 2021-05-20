using FrostLand.Core;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FrostLand.Web.Authentication
{
    public sealed class TokenProvider : ISecurityTokenValidator, ISessionTokenProvider
    {
        public SymmetricSecurityKey Key { get; private set; }
        public string Issuer { get; }

        private readonly IUserSessionService sessionService;

        public bool CanValidateToken => tokenHandler.CanValidateToken;

        public int MaximumTokenSizeInBytes
        {
            get => tokenHandler.MaximumTokenSizeInBytes;
            set => tokenHandler.MaximumTokenSizeInBytes = value;
        }

        private readonly JwtSecurityTokenHandler tokenHandler;

        public TokenProvider(string issuer, IUserSessionService sessionService)
        {
            tokenHandler = new JwtSecurityTokenHandler();
            Issuer = issuer;
            this.sessionService = sessionService;
        }

        public void LoadOrCreateKey()
        {
            var key = new byte[128];
            var keyFile = new FileInfo(Path.Combine(".", "jwt", "issuer.key"));

            if (!keyFile.Exists)
            {
                if (!keyFile.Directory.Exists)
                    keyFile.Directory.Create();

                GenerateNewKey(ref key, 0, key.Length);
                File.WriteAllBytes(keyFile.FullName, key);
            }
            else
            {
                key = File.ReadAllBytes(keyFile.FullName);
            }

            Key = new SymmetricSecurityKey(key);
        }

        public AuthToken Create(string username, bool isRegisterd, Guid session)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                            {
                    new Claim(ClaimTypes.Name, username),
                    new Claim("session", session.ToString()),
                    new Claim("registered", isRegisterd.ToString())
                            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature),
                Issuer = Issuer,
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var strToken = tokenHandler.WriteToken(token);
            return new AuthToken(strToken, token.ValidTo);
        }

        private void GenerateNewKey(ref byte[] key, int offset, int count)
        {
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(key, offset, count);
        }

        public bool CanReadToken(string securityToken)
            => tokenHandler.CanReadToken(securityToken);

        public ClaimsPrincipal ValidateToken(string securityToken,
                TokenValidationParameters validationParameters,
                out SecurityToken validatedToken)
        {
            return tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
        }
    }
}
