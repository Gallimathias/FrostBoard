using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrostLand.WebApi.Extensions
{
    public static class IFileProviderExtension
    {
        public static bool TryGetFileInfo(this IFileProvider fileProvider,
            string subpath, out IFileInfo fileInfo)
        {
            try
            {
                if (subpath.StartsWith("/", StringComparison.Ordinal))
                    subpath = subpath.Substring(1);

                if (!string.IsNullOrWhiteSpace(subpath))
                {
                    fileInfo = fileProvider.GetFileInfo(subpath);
                    return fileInfo.Exists;
                }
            }
            catch (ArgumentException)
            {
            }

            fileInfo = null;
            return false;
        }
    }
}
