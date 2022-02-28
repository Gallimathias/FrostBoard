using FrostLand.Core;
using FrostLand.Model;
using FrostLand.Runtime.Services;
using FrostLand.SqlLite;
using NonSucking.Framework.Extension.IoC;

namespace FrostLand.Runtime
{
    public class RuntimeStartup : IStartUp
    {
        public static void Register(ITypeContainer typeContainer)
        {
            typeContainer.Register<IUserSessionService, UserSessionService>(InstanceBehaviour.Singleton);
            typeContainer.Register<UserSessionService, UserSessionService>(InstanceBehaviour.Singleton);
            typeContainer.Register<FrostLandDbContext, SqlLiteDbContext>();
            typeContainer.Register<StorageProvider>(InstanceBehaviour.Singleton);
        }
    }
}
