using FrostLand.Core;
using FrostLand.Runtime.Services;
using NonSucking.Framework.Extension.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Runtime
{
    public static class Startup
    {
        public static void Register(ITypeContainer typeContainer)
        {
            typeContainer.Register<IUserSessionService, UserSessionService>(InstanceBehaviour.Singleton);
            typeContainer.Register<UserSessionService, UserSessionService>(InstanceBehaviour.Singleton);
        }
    }
}
