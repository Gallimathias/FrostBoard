using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrostLand.Core.Model
{
    public record struct AuthRequest([Required] string Username, [Required] string Password)
    {
    }
}
