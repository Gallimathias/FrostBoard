using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FrostLand.Model.ContentManagement
{
    public class Thread : IndexdEntity<ThreadId>
    {
        public string Name { get; set; }
        public DateTimeOffset Created { get; set; }

        [JsonConverter(typeof(RefListConverter))]
        [InverseProperty(nameof(Post.Parent))]
        public ICollection<Post> Posts { get; set; }

        public Board? Parent { get; set; }

        public Thread()
        {
            Name = "";
            Posts = Array.Empty<Post>();
        }
    }
}
