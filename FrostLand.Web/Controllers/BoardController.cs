using FrostLand.Core;
using FrostLand.Model;
using FrostLand.Model.ContentManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FrostLand.Web.Controllers
{
    [Route("api/[controller]")]    
    public class BoardController : EntityController<Board, BoardId, int>
    {
        public BoardController(StorageProvider storageProvider) : base(storageProvider)
        {
        }
    }
}
