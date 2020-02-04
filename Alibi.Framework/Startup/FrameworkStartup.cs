using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Alibi.Framework.Startup
{
    internal class FrameworkStartup : FrameworkStartupBase
    {
        public FrameworkStartup(IHostEnvironment env) : base(env)
        {
        }

        protected override void AddConfigure(IApplicationBuilder app)
        {
        }

        protected override void AddService(IServiceCollection services)
        {
        }

        protected override void ConfigureJson(JsonOptions jsonOptions)
        {
            base.ConfigureJson(jsonOptions);
            jsonOptions.JsonSerializerOptions.IgnoreNullValues = true;
        }
    }
}