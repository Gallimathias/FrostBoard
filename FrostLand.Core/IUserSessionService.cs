using FrostLand.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Core
{
    public interface IUserSessionService
    {
        SessionContext GuestSession();
        SessionContext Login(string username, string password);
        SessionContext Refresh(SessionContext context);
        void Validate(SessionContext context);
    }
}
