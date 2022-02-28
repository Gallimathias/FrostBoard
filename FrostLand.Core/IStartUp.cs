using NonSucking.Framework.Extension.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Core
{
    public interface IStartUp
    {
        static abstract void Register(ITypeContainer typeContainer);
    }
}
