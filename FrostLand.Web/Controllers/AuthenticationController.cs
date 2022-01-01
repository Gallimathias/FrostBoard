using FrostLand.Core;
using FrostLand.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NonSucking.Framework.Extension.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FrostLand.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase, IDisposable
    {
        private readonly ISessionTokenProvider sessionTokenProvider;
        private readonly ManualResetEvent resetEvent;

        public AuthenticationController(ISessionTokenProvider sessionTokenProvider)
        {
            this.sessionTokenProvider = sessionTokenProvider;

            resetEvent = new ManualResetEvent(false);
        }

        [Route("[action]")]
        [HttpGet]
        public ActionResult Test() 
            => Ok();


        [Route("[action]")]
        [HttpGet]
        public AuthResponse Refresh()
            => sessionTokenProvider.Refresh();

        [Route("login/user")]
        [HttpPost]
        [AllowAnonymous]
        public AuthResponse LoginUser([FromBody] AuthRequest authentication) 
            => sessionTokenProvider.Login(authentication);

        [Route("login/guest")]
        [HttpGet]
        [AllowAnonymous]
        public AuthResponse LoginGuest() 
            => sessionTokenProvider.GuestLogin();

        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult NewGuid()
            => Ok(Guid.NewGuid().ToString("N"));

        public void Dispose()
        {
            resetEvent.Dispose();
        }

    }
}
