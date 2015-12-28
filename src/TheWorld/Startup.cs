﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using TheWorld.Services;

namespace TheWorld
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
           
            #if DEBUG

                 services.AddScoped<IMailService, DebugMailService >();

#else
            services.AddScoped<IMailService, DebugMailService >();
#endif

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            //app.UseDefaultFiles(); // Will look for default files e.g index.html.
            app.UseStaticFiles(); // Will look for static files 

            app.UseMvc
            (
                config=>
                {
                    config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new  { controller= "App", action="Index"}
                    );
                }
            );

            //app.UseIISPlatformHandler();

                        //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync(html);
            //});

           //pp.UsseStaticFiles();
            
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
