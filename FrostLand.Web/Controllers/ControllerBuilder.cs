using FrostLand.Core;
using NonSucking.Framework.Extension.IoC;

namespace FrostLand.Web.Controllers
{
    public partial class ControllerBuilder : IStartUp
    {
        public static void Register(ITypeContainer typeContainer)
        {
            typeContainer.Register<AuthenticationController>();
            typeContainer.Register<BoardController>();
        }

    }
}
