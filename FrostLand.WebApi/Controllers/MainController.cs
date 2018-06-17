using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FrostLand.WebApi.Controllers
{
    [Route("/")]
    public class MainController : Controller
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Index();
        }

        [HttpGet("[action]")]
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }

    }
}
