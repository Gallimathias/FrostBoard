using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrostLand.Web.Model
{
    public sealed class AuthResponse
    {
        public Guid Session { get; set; }
        public string Token { get; set; }
        public DateTimeOffset ExpireDate { get; internal set; }
    }
}
