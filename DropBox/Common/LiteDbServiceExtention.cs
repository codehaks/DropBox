using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DropBox.Common
{
    public static class LiteDbServiceExtention
    {
        public static void AddLiteDb(this IServiceCollection services,string databasePath)
        {
            services.AddTransient<LiteDbContext, LiteDbContext>();
            services.Configure<LiteDbConfig>(options => options.DatabasePath = databasePath);
        }
    }
}
