using FrostLand.Core;
using FrostLand.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FrostLand.Web.Controllers
{
    [ApiController]
    [Authorize]
    public class EntityController<TEntity, TKey, TApiKey> : ControllerBase where TEntity : Entity
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

        [HttpGet("{id}")]
        public IActionResult Get(TApiKey id)
        {
            var value = storageProvider.Find<TEntity, TKey>(GenericCaster<TApiKey, TKey>.Cast(id));

            if (value is null)
                return StatusCode((int)HttpStatusCode.NotFound);

            return Ok(value);
        }

        [HttpGet("[action]")]
        public Paginator Paginate([FromQuery] int size = 10)
            => storageProvider.GetPaginator<TEntity>(size);

        [HttpDelete("{id}")]
        public IActionResult Remove(TApiKey id)
        {
            var state = storageProvider.Remove<TEntity, TKey>(GenericCaster<TApiKey, TKey>.Cast(id));

            return
                state == EntityState.Deleted
                ? Ok()
                : StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}