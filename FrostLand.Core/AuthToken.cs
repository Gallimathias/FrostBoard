using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Core
{
    public struct AuthToken
    {        
        public string Token { get; set; }
        public DateTimeOffset ExpireDate { get; set; }

        public AuthToken(string token, DateTimeOffset expireDate)
        {
            Token = token;
            ExpireDate = expireDate;
        }

    }
}
