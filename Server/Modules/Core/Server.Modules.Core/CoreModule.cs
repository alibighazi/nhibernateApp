using Alibi.Framework.Interceptor;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Server.Modules.Core.Business;
using Server.Modules.Core.Common.Interfaces;
using Server.Modules.Core.Controllers;

namespace Server.Modules.Core
{
    public class CoreModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {


            builder.RegisterType<BookBusiness>().As<IBookBusiness>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(TransactionInterceptor));


            builder.RegisterType<BookController>()
                .PropertiesAutowired()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(AuthenticatedUserInterceptor));
        }
    }
}
