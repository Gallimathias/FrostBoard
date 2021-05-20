using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrostLand.Model;
using FrostLand.Web.Generators;

namespace FrostLand.Web.Controllers
{
    [ControllerGenerator]
    public static partial class ControllerBuilder
    {
        static partial void BuildOf(Board board);
    }
}
