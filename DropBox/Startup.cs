using DropBox.Common;
using DropBox.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DropBox
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLiteDb(_env.ContentRootPath+ "/bug.litedb");
            services.AddSignalR();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<UploadHub>("/uploadhub");
                //routes.MapHub<StatusHub>("/statusHub");
            });
            app.UseMvcWithDefaultRoute();
        }
    }
}
