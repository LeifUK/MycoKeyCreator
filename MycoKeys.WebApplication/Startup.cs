using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MycoKeys.WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment iWebHostEnvironment)
        {
            IConfiguration = configuration;
            IWebHostEnvironment = iWebHostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            System.Data.Common.DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
#if DEBUG
            if (IWebHostEnvironment.IsDevelopment())
            {
                services.AddControllersWithViews().AddRazorRuntimeCompilation();
                services.AddRazorPages().AddRazorRuntimeCompilation();
                services.AddMvc().AddRazorRuntimeCompilation();
            }
#endif
            services.Add(new ServiceDescriptor(typeof(Services.IKeyManagerFactory), new Services.KeyManagerFactory()));
        }
        
        private IWebHostEnvironment IWebHostEnvironment;
        private Microsoft.Extensions.Configuration.IConfiguration IConfiguration;

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            System.Diagnostics.Trace.TraceWarning("Startup.Configure()");

            IWebHostEnvironment = env;
            if (IWebHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate, max-age=0");
                    context.Context.Response.Headers.Add("Expires", "-1");
                }
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                    System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"wwwroot/css")),
                RequestPath = "/CSS"
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Keys}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                        name: "Keys",
                        pattern: "{controller=Keys}/{action=Index}/{id}");

                endpoints.MapControllerRoute(
                    name: "About",
                    pattern: "About",
                    defaults: new { controller = "Keys", action = "About" });

                endpoints.MapControllerRoute(
                    name: "MycoKeyMaker",
                    pattern: "MycoKeyMaker",
                    defaults: new { controller = "Keys", action = "MycoKeyMaker" });

                endpoints.MapControllerRoute(
                    name: "update",
                    pattern: "{controller=Keys}/{action=Update}");

                endpoints.MapControllerRoute(
                    name: "KeyMatch",
                    pattern: "Keys/{keyName}",
                    defaults: new { controller = "Keys", action = "KeyMatch" });
            }
            );
        }
    }
}
