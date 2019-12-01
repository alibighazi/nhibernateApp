using Alibi.Framework.DbContext;
using Alibi.Framework.Models;
using Autofac;
using Autofac.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Alibi.Framework.Srartup
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddAlibiFrameworkDependacies(this IServiceCollection services, IConfiguration Configuration)
        {

            services.AddControllers();
            services.AddCors();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static ContainerBuilder AddContainerModules(this ContainerBuilder builder, IConfiguration Configuration)
        {

            #region -- load autofac modules ---------------------------
            var type = typeof(Module);
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
            // load mapping class assembly string
            var mappingsAssembly = Configuration.GetSection("Mapping").Get<IList<string>>();
            // get connectionstring
            var connStr = Configuration.GetConnectionString("DefaultConnection");
            // create nhibernate session
            builder.AddNHibernate(connStr, mappingsAssembly);
            #endregion

            return builder;

        }
    }
}
