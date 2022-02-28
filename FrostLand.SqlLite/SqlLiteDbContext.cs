using FrostLand.Core;
using FrostLand.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrostLand.SqlLite
{
    public class SqlLiteDbContext : FrostLandDbContext, IEnsureDatabase
    {
        private readonly IConfiguration configuration;

        public SqlLiteDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={configuration.GetConnectionString("default")}");
            base.OnConfiguring(optionsBuilder);
        }

        public static FrostLandDbContext GetEnsureDatabase(IConfiguration settings)
        {
            var db = new SqlLiteDbContext(settings);
            db.Database.EnsureCreated();
            return db;
        }
    }
}
