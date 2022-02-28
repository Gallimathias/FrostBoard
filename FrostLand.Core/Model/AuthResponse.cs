using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrostLand.Core.Model
{
    public record struct AuthResponse(Guid Session, string Token, DateTimeOffset ExpireDate)
    {
    }
}
