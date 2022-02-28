using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Model.ContentManagement
{
    public class Post : IndexdEntity<PostId>
    {
        public Thread? Parent { get; set; }
    }
}
