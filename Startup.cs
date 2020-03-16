using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QAP4.Repository;
using QAP4.Models;
using QAP4.Middleware;
using QAP4.Extensions;
using Microsoft.Extensions.Hosting;
using QAP4.Infrastructure.CrossCutting;

namespace QAP4
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
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
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddControllers(options => options.EnableEndpointRouting = false);

            // add DB and repository parttern
            services.AddSingleton<IConfiguration>(Configuration);
            RegisterServices(services);

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                // options.CookieHttpOnly = true;
                // options.CookieName = ".MyApplication";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            

            // Add cors befor add MVC
            app.UseCors(AppAllowSpecificOrigins);

            //session
            app.UseSession();

            // app.UseBrowserLink();

            //route
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            //web socket
            //app.UseWebSockets();
            //app.UseMiddleware<ChatWebSocketMiddleware>();

            //mvc route
            app.UseMvcWithDefaultRoute();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            NativeInjectorBootStrapper.Register(services);
        }
    }
}
