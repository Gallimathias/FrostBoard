using FrostLand.Core;
using FrostLand.Core.Model;
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
        private readonly ConcurrentDictionary<Guid, SessionContext> activeSessions;

        public UserSessionService()
        {
            activeSessions = new ConcurrentDictionary<Guid, SessionContext>();
        }

        public SessionContext Login(string username, string password)
        {
            return CreateNewSession(username, 0, true);
        }

        public SessionContext GuestSession()
        {
            return CreateNewSession("_GUEST", -1, false);
        }

        public SessionContext Refresh(SessionContext context)
        {
            if (!activeSessions.TryRemove(context.SessionId, out SessionContext oldSession))
            {
                throw new AuthenticationException();
            }

            return CreateNewSession(context.Username, context.UserId, context.IsRegistered);
        }

        public void Validate(SessionContext context)
        {
            if(activeSessions.TryGetValue(context.SessionId, out var expectedContext)){

                if (expectedContext == context)
                    return;
            }

            throw new AuthenticationException();
        }

        private SessionContext CreateNewSession(string username, int id, bool isRegistered)
        {
            Guid newSessionId = Guid.NewGuid();
            SessionContext session = new(
                isRegistered: isRegistered,
                sessionId: newSessionId,
                userId: id,
                username: username
            );

            if (!activeSessions.TryAdd(newSessionId, session))
            {
                throw new AuthenticationException();
            }

            return session;
        }
    }

}
