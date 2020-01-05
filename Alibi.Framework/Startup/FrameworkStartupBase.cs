using Alibi.Framework.DbContext;
using Alibi.Framework.Middleware;
using Alibi.Framework.Models;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Alibi.Framework.Startup
{
    public abstract class FrameworkStartupBase
    {
        private IConfiguration Configuration { get; }
        private ILifetimeScope AutofacContainer { get; set; }


        protected FrameworkStartupBase(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region -- load controllers class ------------------------

            // TODO: remove controller assembly and move them into one assembly with business and other stuff
            var mappingsAssembly = Configuration.GetSection("Modules").Get<IList<string>>();
            foreach (var item in mappingsAssembly)
            {
                var assembly = Assembly.Load(item + ".Controller");
                services.AddControllers()
                    .AddApplicationPart(assembly);
            }

            #endregion


            services.AddControllers()
                .AddFluentValidation()
                .AddControllersAsServices()
                .AddJsonOptions(ConfigureJson);

            services.AddCors();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            #region -- configure jwt authentication-------------------

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
#if DEBUG
                    x.RequireHttpsMetadata = false;
#endif
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            #endregion

            services.AddAuthorization();
            AddService(services);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region -- load modules class assembly -------------------

            var mappingsAssembly = Configuration.GetSection("Modules").Get<IList<string>>();
            foreach (var item in mappingsAssembly)
            {
                Assembly.Load(item);
                Assembly.Load(item + ".Mapping");
            }

            #endregion

            #region -- load autofac modules ---------------------------

            builder.RegisterModule(new FrameworkModule());

            var type = typeof(Autofac.Module);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.FullName != null && (type.IsAssignableFrom(p) && p.FullName.ToLower().Contains("server")))
                .ToList();

            foreach (var instance in types.Select(item => (IModule) Activator.CreateInstance(item)))
            {
                builder.RegisterModule(instance);
            }

            #endregion

            #region -- nhibernate ---------------------------------------

            // get the connection string
            var connStr = Configuration.GetConnectionString("DefaultConnection");
            // create nhibernate session
            builder.AddNHibernate(connStr);

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();


            if (env.IsDevelopment())
            {
                app.UseMiddleware<ExceptionHandler>();
            }
            else
            {
                app.UseMiddleware<ExceptionHandler>();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            // use angular
            app.UseStaticFiles();
            app.UseRouting();


            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


            app.UseAuthentication();
            app.UseAuthorization();


            //app.UseResponseCompression();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


            AddConfigure(app);
        }


        protected abstract void AddConfigure(IApplicationBuilder app);
        protected abstract void AddService(IServiceCollection services);
        protected virtual void ConfigureJson(JsonOptions jsonOptions)
        {
            jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            jsonOptions.JsonSerializerOptions.WriteIndented = true;
        }
    }
}