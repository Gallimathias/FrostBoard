using FrostLand.Core;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace FrostLand.Model
{
    public abstract class FrostLandDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AssemblyLoadContext
                .Default
                .Assemblies
                .Where(assembly => assembly.GetName().Name?.StartsWith(nameof(FrostLand)) ?? false)
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(Entity).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                .ForEach(type =>
                {
                    if (modelBuilder.Model.FindEntityType(type) is null)
                        _ = modelBuilder.Model.AddEntityType(type);
                });
        }
    }
}
