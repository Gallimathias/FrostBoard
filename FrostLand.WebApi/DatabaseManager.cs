using FrostLand.Model;
using FrostLand.SqlLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrostLand.WebApi
{
    internal static class DatabaseManager
    {
        public static FrostlandDatabase MainDatabase { get; private set; }

        static DatabaseManager(){
            //TODO: Check config, get dll and create db
            //SqlLite is default
            MainDatabase = FrostLandSqlLiteDatabase.GetDatabase("FrostLand_Main.db");
            MainDatabase.Database.EnsureCreated();
        }

        internal static void Initialize()
        {
            
        }
    }
}
