using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Model.UserManagment
{
    [Index(nameof(Username), IsUnique = true)]
    public class User : IndexdEntity<UserId>
    {       
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Secret { get; set; }

        public User()
        {
            Username = "";
            Secret = "";
        }
    }
}
