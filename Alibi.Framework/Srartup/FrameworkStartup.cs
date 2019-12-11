using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Alibi.Framework.Srartup
{
    class FrameworkStartup : FrameworkStartupBase
    {


        public FrameworkStartup(IWebHostEnvironment env): base(env)
        {

        }
        public override void AddConfigure(IApplicationBuilder app)
        {
        }

        public override void AddService(IServiceCollection services)
        {
        }
    }
}
