﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using apis.Models;
using apis.Data;
using Newtonsoft.Json.Serialization;

namespace apis
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddControllersAsServices().AddJsonOptions(opts =>
            {
                // Force Camel Case to JSON
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddTransient<IUserStore<Models.User>, UserStore<Models.User, IdentityRole<Int32>, NgContext, Int32>>();

            services.AddTransient<IRoleStore<IdentityRole<Int32>>, RoleStore<IdentityRole<Int32>, NgContext, Int32>>();

            services.AddTransient<IRepository<Recette>, RecetteRepository>();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddEntityFramework()
                .AddDbContext<NgContext>();

            services.AddTransient<INgContext, NgContext>();

            services.AddIdentity<Models.User, IdentityRole<Int32>>(sa =>
            {
                sa.Password.RequireDigit = false;
                sa.Password.RequireUppercase = false;
                sa.Password.RequiredLength = 0;
                sa.Password.RequireNonAlphanumeric = false;
                sa.Cookies.ApplicationCookie.LoginPath = "/App/Login";
                sa.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(1);
            });

            services.AddTransient<IConfigurationRoot>(s => { return Configuration; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {   
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //app.UseMiddleware<LoggingMiddleware>();

            app.UseCors(builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().Build());

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc();            
        }
    }
}
