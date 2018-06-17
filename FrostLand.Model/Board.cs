using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FrostLand.Model
{
    public class Board
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        
        [JsonIgnore]
        public virtual Board Parent { get; set; }

        [InverseProperty("Board")]
        [JsonIgnore]
        public virtual ICollection<Thread> Threads { get; set; }

        [InverseProperty("Parent")]
        [JsonIgnore]
        public virtual ICollection<Board> SubBoards { get; set; }

        [NotMapped]
        [JsonProperty("parent")]
        public SubBoard ParentBoard { get => Parent != null ? new SubBoard(Parent.Id, Parent.Name) : null; set => Parent = database.Boards.First(b => b.Id == value.Id); }

        [NotMapped]
        [JsonProperty("threads")]
        public virtual List<ThreadRef> ThreadRefs { get => Threads?.Select(t => new ThreadRef(t.Id)).ToList(); set => AddThreads(value); }

        [NotMapped]
        [JsonProperty("subBoards")]
        public virtual List<SubBoard> SubBoardsId { get => SubBoards?.Select(b => new SubBoard(b.Id, b.Name)).ToList(); set => AddSubboards(value); }

        private FrostlandDatabase database;

        public Board()
        {
            database = FrostlandDatabase.DatabaseRegister.FirstOrDefault();
        }

        private void AddSubboards(List<SubBoard> subBoards)
        {
            if (SubBoards == null)
                SubBoards = new HashSet<Board>();

            foreach (var subBoard in subBoards)
            {
                var board = database
                                .Boards
                                .FirstOrDefault(b => b.Id == subBoard.Id);

                if (!SubBoards.Contains(board))
                    SubBoards.Add(board);
            }
        }

        private void AddThreads(List<ThreadRef> threadRefs)
        {
            if (Threads == null)
                Threads = new HashSet<Thread>();

            foreach (var threadRef in threadRefs)
            {
                var thread = database
                                .Threads
                                .FirstOrDefault(b => b.Id == threadRef.Id);

                if (!Threads.Contains(thread))
                    Threads.Add(thread);
            }
        }

        public class SubBoard
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public SubBoard(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        public class ThreadRef
        {
            public int Id { get; set; }

            public ThreadRef(int id)
            {
                Id = id;
            }
        }
    }
}
