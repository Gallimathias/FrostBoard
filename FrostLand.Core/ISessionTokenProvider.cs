using FrostLand.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Core
{
    public interface ISessionTokenProvider
    {
        AuthToken Create(SessionContext context);
        AuthResponse GuestLogin();
        AuthResponse Login(AuthRequest authentication);
        AuthResponse Refresh();
        AuthResponse Register(AuthRequest authentication);
    }
}
