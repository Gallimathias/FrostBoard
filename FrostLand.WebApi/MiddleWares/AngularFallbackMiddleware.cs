using FrostLand.WebApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrostLand.WebApi.MiddleWares
{
    public class AngularFallbackMiddleware
    {
        private readonly FileServerOptions options;
        private readonly RequestDelegate next;

        public AngularFallbackMiddleware(RequestDelegate next, FileServerOptions options)
        {
            this.options = options;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!TryMatchPath(context, options.RequestPath, false, out PathString subpath) ||
                !options.FileProvider.TryGetFileInfo(subpath.Value, out IFileInfo fileInfo))
            {
                context.Request.Path = new PathString(options.RequestPath + "/index.html");
            }

            await next.Invoke(context);
        }

        internal static bool PathEndsInSlash(PathString path)
            => path.Value.EndsWith("/", StringComparison.Ordinal);

        internal static bool TryMatchPath(HttpContext context, PathString matchUrl, bool forDirectory, out PathString subpath)
        {
            var path = context.Request.Path;

            if (forDirectory && !PathEndsInSlash(path))
            {
                path += new PathString("/");
            }

            if (path.StartsWithSegments(matchUrl, out subpath))
            {
                return true;
            }

            return false;
        }

    }
}
