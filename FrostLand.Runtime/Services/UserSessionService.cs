using FrostLand.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Runtime.Services
{
    public class UserSessionService : IUserSessionService
    {

        public UserSessionService()
        {
        }

        public Guid Login(string username, string password)
        {
            return Guid.NewGuid();
        }

        public Guid GuestSession()
        {
            return Guid.NewGuid();
        }

        public Guid Refresh()
        {
            return Guid.NewGuid();
        }
    }

}
