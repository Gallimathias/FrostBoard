using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Core.Model
{
    public record struct SessionContext(string Username, int UserId, Guid SessionId, bool IsRegistered)
    {              
    }
}
