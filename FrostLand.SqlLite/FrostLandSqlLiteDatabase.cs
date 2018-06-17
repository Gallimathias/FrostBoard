using FrostLand.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrostLand.SqlLite
{
    public class FrostLandSqlLiteDatabase : FrostlandDatabase
    {
        static FrostLandSqlLiteDatabase() => SQLitePCL.Batteries.Init();

        public FrostLandSqlLiteDatabase(DbContextOptions options) : base(options)
        {
            
        }

        public override FrostlandDatabase GetEnsureDatabase(string source)
        {
            var db = GetDatabase(source);            
            db.Database.EnsureCreated();
            return db;
        }

        public static FrostLandSqlLiteDatabase GetDatabase(string source)
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseSqlite($"Data Source={source}");
            return new FrostLandSqlLiteDatabase(builder.Options);
        }
    }
}
