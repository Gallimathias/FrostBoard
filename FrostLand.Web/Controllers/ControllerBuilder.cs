using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrostLand.Model;
using NonSucking.Framework.Extension.IoC;
//using FrostLand.Web.Generators;

namespace FrostLand.Web.Controllers
{
    //[ControllerGenerator]
    public static partial class ControllerBuilder
    {
        public static void Register(ITypeContainer typeContainer)
        {
            typeContainer.Register<AuthenticationController>();
            typeContainer.Register<BoardController>();
        }

        //static partial void BuildOf(Board board);
    }
}
