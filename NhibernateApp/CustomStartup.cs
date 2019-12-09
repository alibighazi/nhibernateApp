using Alibi.Framework.Srartup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace NhibernateApp
{
    public class CustomStartup : FrameworkStartupBase
    {

        public CustomStartup(IWebHostEnvironment env) : base(env)
        { }



        public override void AddConfigure(IApplicationBuilder app)
        {
            app.UseHealthChecks("/health");
        }

        public override void AddService(IServiceCollection services)
        {
            services.AddHealthChecks();
        }
    }
}
