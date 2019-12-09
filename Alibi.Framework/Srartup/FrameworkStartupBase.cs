using Alibi.Framework.DbContext;
using Alibi.Framework.Models;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

namespace Alibi.Framework.Srartup
{
    public abstract class FrameworkStartupBase
    {

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }


        public FrameworkStartupBase(IWebHostEnvironment env)
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
            var mappingsAssembly = Configuration.GetSection("Modules").Get<IList<string>>();
            foreach (var item in mappingsAssembly)
            {
                var assembly = Assembly.Load(item + ".Controller");
                services.AddControllers()
                    .AddApplicationPart(assembly);
            }
            #endregion

            services.AddControllers()
                .AddControllersAsServices()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    o.JsonSerializerOptions.WriteIndented = true;

                });

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
                .Where(p => type.IsAssignableFrom(p) && p.FullName.ToLower().Contains("server")).ToList();

            foreach (var item in types)
            {
                IModule instance = (IModule)Activator.CreateInstance(item);
                builder.RegisterModule(instance);
            }
            #endregion

            #region -- nhibernate ---------------------------------------

            // get connectionstring
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
                app.UseDeveloperExceptionPage();
            }

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            AddConfigure(app);


        }


        public abstract void AddConfigure(IApplicationBuilder app);
        public abstract void AddService(IServiceCollection services);




    }

}
