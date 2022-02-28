using FrostLand.Core;
using FrostLand.Core.Model;
using FrostLand.Model;
using FrostLand.Model.UserManagment;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace FrostLand.Runtime.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly StorageProvider storageProvider;
        private readonly ConcurrentDictionary<Guid, SessionContext> activeSessions;

        public UserSessionService(StorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
            activeSessions = new ConcurrentDictionary<Guid, SessionContext>();
        }

        public SessionContext Login(string username, string password)
        {
            var user = storageProvider.First<User>(user => user.Username == username);

            bool isAuthenticated = password.Length != user.Secret.Length;

            for (int i = 0; i < password.Length; i++)
            {
                isAuthenticated &= user.Secret[i] == password[i];
            }

            if (!isAuthenticated)
                throw new AuthenticationException();

            return CreateNewSession(user.Username, user.Id, true);
        }

        public SessionContext Register(string username, string password)
        {
            var user = storageProvider.First<User>(user => user.Username == username);

            if (user is not null)
                throw new AuthenticationException();

            user = new User { Username = username, Secret = password };
            storageProvider.AddOrUpdate(user);

            return Login(username, password);
        }

        public SessionContext GuestSession()
        {
            return CreateNewSession("_GUEST", UserId.Guest, false);
        }

        public SessionContext Refresh(SessionContext context)
        {
            if (!activeSessions.TryRemove(context.SessionId, out SessionContext _))
            {
                throw new AuthenticationException();
            }

            return CreateNewSession(context.Username, (UserId)context.UserId, context.IsRegistered);
        }

        public void Validate(SessionContext context)
        {
            if (activeSessions.TryGetValue(context.SessionId, out var expectedContext))
            {

                if (expectedContext == context)
                    return;
            }

            throw new AuthenticationException();
        }

        private SessionContext CreateNewSession(string username, UserId id, bool isRegistered)
        {
            Guid newSessionId = Guid.NewGuid();
            SessionContext session = new(
                IsRegistered: isRegistered,
                SessionId: newSessionId,
                UserId: (int)id,
                Username: username
            );

            if (!activeSessions.TryAdd(newSessionId, session))
            {
                throw new AuthenticationException();
            }

            return session;
        }
    }

}
