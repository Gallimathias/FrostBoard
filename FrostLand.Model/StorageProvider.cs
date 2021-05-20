using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Model
{
    public class StorageProvider
    {
        private readonly DbContext database;

        public StorageProvider()
        {
            database = default;
        }

        public EntityState AddOrUpdate<T>(T entity) where T : Entity
        {
            var dbSet = database.Set<T>();
            EntityEntry<T> entry;
            if (dbSet.Contains(entity))
                entry = dbSet.Update(entity);
            else
                entry = dbSet.Add(entity);

            database.SaveChanges();
            return entry.State;
        }

        public T Find<T, K>(K key) where T : Entity
        {
            var dbSet = database.Set<T>();
            return dbSet.Find(key);
        }

        public EntityState Remove<T>(T entity) where T : Entity
        {
            var dbSet = database.Set<T>();
            var entry = dbSet.Remove(entity);
            database.SaveChanges();
            return entry.State;
        }
    }
}
