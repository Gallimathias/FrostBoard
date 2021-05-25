using FrostLand.Core;
using FrostLand.Web.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NonSucking.Framework.Extension.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrostLand.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserSessionService userSessionService;

        public AuthenticationController(IUserSessionService userSessionService)
        {
            this.userSessionService = userSessionService;
        }

        [Route("[action]")]
        [HttpGet]
        public ActionResult Test() 
            => Ok();


        [Route("[action]")]
        [HttpGet]
        public AuthResponse Refresh()
            => Ok();

        [Route("login/user")]
        [HttpPost]
        [AllowAnonymous]
        public AuthResponse LoginUser([FromBody] AuthRequest authentication)
        {
            userSessionService.Login(authentication.Username, authentication.Password);
            return Ok();
        }

        [Route("login/guest")]
        [HttpGet]
        [AllowAnonymous]
        public AuthResponse LoginGuest()
        {
            userSessionService.GuestSession();
            return Ok();
        }

        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult NewGuid()
            => Ok(Guid.NewGuid().ToString("N"));
    }
}
