using FrostLand.WebApi.MiddleWares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrostLand.WebApi.Extensions
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder UseAngularFallback(this IApplicationBuilder app, FileServerOptions options)
        {

            return app.UseMiddleware<AngularFallbackMiddleware>(options);
        }
    }
}
