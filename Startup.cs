using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QAP4.Infrastructure.Repositories;
using QAP4.Models;
using QAP4.Middleware;
using QAP4.Extensions;
using QAP4.Infrastructure.Extensions;
using AutoMapper;
using QAP4.Infrastructure.CrossCutting;

namespace QAP4
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        readonly string AppAllowSpecificOrigins = "_appAllowSpecificOrigins";
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Cors rules
            services.AddCors(options =>
            {
                options.AddPolicy(AppAllowSpecificOrigins, builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    //.AllowCredentials();
                });
            });


            // Add framework services.
            services.AddMvc();
            
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                // options.CookieHttpOnly = true;
                // options.CookieName = ".MyApplication";
            });

            // add DB and repository parttern
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<QAPContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
    
            services.AddAutoMapper();

            // Native DI Abstraction
            NativeInjectorBootStrapper.Register(services);

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Add cors befor add MVC
            app.UseCors(AppAllowSpecificOrigins);

            //session
            app.UseSession();

            //route
            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            app.UseStaticFiles();

            //web socket
            //app.UseWebSockets();
            //app.UseMiddleware<ChatWebSocketMiddleware>();

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=Home}/{action=Index}/{id?}");
            // });

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=User}/{action=Index}/{id?}");
            // });

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=Group}/{action=Index}/{id?}");
            // });

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=Posts}");
            // });

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=Question}/{action=Index}/{id?}");
            // });

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=Tutorial}/{action=Index}/{id?}");
            // });

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=Test}/{action=Index}/{id?}");
            // });

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=Tag}/{action=Index}/{id?}");
            // });

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=Search}/{action=Index}/{id?}");
            // });


            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=x}/{action=Index}/{id?}");
            // });

            //error handler
            //app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            //mvc route
            app.UseMvcWithDefaultRoute();
        }
    }
}
