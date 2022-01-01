using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Core.Model
{
    public readonly struct SessionContext : IEquatable<SessionContext>
    {       
        public string Username { get;  }
        public int UserId { get;  }
        public Guid SessionId { get;  }
        public bool IsRegistered { get;  }

        public SessionContext(string username, int userId, Guid sessionId, bool isRegistered)
        {
            Username = username;
            UserId = userId;
            SessionId = sessionId;
            IsRegistered = isRegistered;
        }

        public override bool Equals(object obj)
        {
            return obj is SessionContext context && Equals(context);
        }

        public bool Equals(SessionContext other)
        {
            return Username == other.Username &&
                   UserId == other.UserId &&
                   SessionId.Equals(other.SessionId) &&
                   IsRegistered == other.IsRegistered;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Username, UserId, SessionId, IsRegistered);
        }

        public static bool operator ==(SessionContext left, SessionContext right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SessionContext left, SessionContext right)
        {
            return !(left == right);
        }
    }
}
