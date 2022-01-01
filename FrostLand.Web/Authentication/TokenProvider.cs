using FrostLand.Core;
using FrostLand.Core.Model;
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
                if (!keyFile.Directory!.Exists)
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

        public AuthResponse Login(AuthRequest authentication)
        {
            var result = sessionService.Login(authentication.Username, authentication.Password);
            var token = Create(result);
            return new()
            {
                Session = result.SessionId,
                Token = token.Token,
                ExpireDate = token.ExpireDate
            };
        }
        public AuthResponse GuestLogin()
        {
            var result = sessionService.GuestSession();
            var token = Create(result);
            return new()
            {
                Session = result.SessionId,
                Token = token.Token,
                ExpireDate = token.ExpireDate
            };
        }

        public AuthResponse Refresh()
        {
            var result = sessionService.GuestSession();
            var token = Create(result);
            return new()
            {
                Session = result.SessionId,
                Token = token.Token,
                ExpireDate = token.ExpireDate
            };
        }

        public AuthToken Create(SessionContext context)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.Name, context.Username),
                                new Claim(ClaimTypes.Sid, context.UserId.ToString()),
                                new Claim("session", context.SessionId.ToString()),
                                new Claim("registered", context.IsRegistered.ToString())
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

        private static void GenerateNewKey(ref byte[] key, int offset, int count)
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
            var principal = tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
            var context = GetContextFrom(principal);
            sessionService.Validate(context);
            return principal;
        }

        public SessionContext GetContextFrom(ClaimsPrincipal principal)
        {
            var username = principal.FindFirstValue(ClaimTypes.Name);
            var userId = principal.FindFirstValue(ClaimTypes.Sid);
            var session = principal.FindFirstValue("session");
            var registered = principal.FindFirstValue("registered");

            return new SessionContext
            (
                username: username,
                userId: int.Parse(userId),
                sessionId: Guid.Parse(session),
                isRegistered: bool.Parse(registered)
            );
        }
    }
}
