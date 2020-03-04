using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Extensions.Convert.ConvertHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UserManager.API.Db;
using UserManager.API.Model;
using IApplicationLifetime = Microsoft.Extensions.Hosting.IApplicationLifetime;

namespace UserManager.API
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            ILogger<Startup> logger,
            IHostingEnvironment env
            )
        {
            Configuration = configuration;
            Logger = logger;
            Env = env;
            IsApollo = Env.IsProduction();
        }

        public bool IsApollo { get; set; }
        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }

        public IHostingEnvironment Env { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // 认证 部分  放置在 网关部分
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("Mysql"), mysqloptions =>
                {
                    mysqloptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                }));
            //  
            services.AddIdentity<UserIdentity, UserIdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddCors(options =>
            {
                options.AddPolicy("spa", config =>
                {
                    config
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyOrigin();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifetime)
        {
            app.UseCors("spa");
            app.UseMvc();
        }
    }
}
