using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Model.ContentManagement
{
    public class Board : IndexdEntity<BoardId>
    {
        public string Name { get; set; }

        [JsonConverter(typeof(RefListConverter))]
        [InverseProperty(nameof(Thread.Parent))]
        public ICollection<Thread> Threads { get; set; }

        [JsonConverter(typeof(RefListConverter))]
        [InverseProperty(nameof(Parent))]
        public ICollection<Board> SubBoards { get; set; }

        public Board? Parent { get; set; }

        public Board()
        {
            Name = "";
            Threads = Array.Empty<Thread>();
            SubBoards = Array.Empty<Board>();
        }
    }
}
