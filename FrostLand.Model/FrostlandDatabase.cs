using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FrostLand.Model
{
    public abstract class FrostlandDatabase : DbContext
    {
        public static List<FrostlandDatabase> DatabaseRegister { get; private set; }

        public DbSet<Board> Boards { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Thread> Threads { get; set; }

        static FrostlandDatabase()
        {
            DatabaseRegister = new List<FrostlandDatabase>();
        }

        public FrostlandDatabase() : base()
        {
            DatabaseRegister.Add(this);
        }
        public FrostlandDatabase(DbContextOptions options) : base(options)
        {
            DatabaseRegister.Add(this);
        }

        public abstract FrostlandDatabase GetEnsureDatabase(string source);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        ~FrostlandDatabase()
        {
            DatabaseRegister.Remove(this);
        }
    }
}
