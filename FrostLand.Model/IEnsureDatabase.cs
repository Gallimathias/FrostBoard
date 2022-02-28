using Microsoft.Extensions.Configuration;

namespace FrostLand.Model
{
    public interface IEnsureDatabase
    {
        public static abstract FrostLandDbContext GetEnsureDatabase(IConfiguration settings);
    }
}