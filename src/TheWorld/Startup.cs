using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using TheWorld.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using TheWorld.Controllers.Api;
using TheWorld.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TheWorld
{
    public class Startup
    {
        public static IConfigurationRoot Confgiuration;

        public Startup(IApplicationEnvironment appEnv )
        {
            var bulder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            Confgiuration = bulder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(opt=>
                {
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.AddIdentity<WorldUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
            }).AddEntityFrameworkStores<WorldContext>();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<WorldContext>();

            services.AddScoped<CoordService>();
            services.AddTransient<WorldContextSeedData>();
            services.AddScoped<IWorldRepository, WorldRepository>();
            services.AddLogging();

#if DEBUG

            services.AddScoped<IMailService, DebugMailService >();

#else
            services.AddScoped<IMailService, DebugMailService >();
#endif

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void  Configure(IApplicationBuilder app, WorldContextSeedData seeder, ILoggerFactory loggerFactory)
        {
            //app.UseDefaultFiles(); // Will look for default files e.g index.html.
            app.UseStaticFiles(); // Will look for static files 

            app.UseIdentity();

            AutoMapper.Mapper.Initialize(config=>
            {
                config.CreateMap<Trip, TripViewModel>().ReverseMap();
                config.CreateMap<Stop, StopViewModel>().ReverseMap();
            });

            loggerFactory.AddDebug(LogLevel.Warning);

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

          await seeder.EnsureSeedDataAsync();

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
