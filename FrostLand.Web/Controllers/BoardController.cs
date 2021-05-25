using FrostLand.Core;
using FrostLand.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FrostLand.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BoardController : EntityController<Board, int>
    {
        public BoardController(StorageProvider storageProvider) : base(storageProvider)
        {
        }
    }
}
