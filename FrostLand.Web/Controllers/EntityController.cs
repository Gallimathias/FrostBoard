using FrostLand.Core;
using FrostLand.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FrostLand.Web.Controllers
{
    public abstract class EntityController<TEntity, TKey> : ControllerBase where TEntity : Entity
    {
        private readonly StorageProvider storageProvider;

        public EntityController(StorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        [HttpPost]
        public IActionResult AddOrUpdate(TEntity value) 
        {
            var state = storageProvider.AddOrUpdate(value);

            return
                (state == EntityState.Added || state == EntityState.Modified)
                ? Ok()
                : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet]
        public Page<TEntity> Get([FromQuery] int index = 0, [FromQuery] int size = 10)
            => storageProvider.Get<TEntity>(index, size);

        [HttpGet("{id:int}")]
        public TEntity Get(TKey id)
            => storageProvider.Find<TEntity, TKey>(id);

        [HttpGet("[action]")]
        public Paginator Paginate([FromQuery] int size = 10)
            => storageProvider.GetPaginator<TEntity>(size);

        [HttpDelete("{id:int}")]
        public IActionResult Remove(TKey id)
        {
            var state = storageProvider.Remove<TEntity, TKey>(id);

            return
                state == EntityState.Deleted
                ? Ok()
                : StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}