using Alibi.Framework.DbContext;
using Autofac;
using Server.Modules.Core.Business;
using Server.Modules.Core.Common.Interfaces;
using Server.Modules.Core.Common.Models;
using Server.Modules.Core.Mapping;
using System.Reflection;

namespace Server.Modules.Core
{
    public class CoreModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            // The generic ILogger<TCategoryName> service was added to the ServiceCollection by ASP.NET Core.
            // It was then registered with Autofac using the Populate method. All of this starts
            // with the services.AddAutofac() that happens in Program and registers Autofac
            // as the se    rvice provider.
            //builder.Register(c => new ValuesService(c.Resolve<ILogger<ValuesService>>()))
            //    .As<IValuesService>()
            //    .InstancePerLifetimeScope();

            builder.RegisterType<BookBusiness>().As<IBookBusiness>();
            builder.RegisterType<BookMap>().As<IMap>();
            builder.RegisterType<NHibernateMapperSession<Book>>().As<IMapperSession<Book>>();

            Assembly.Load("Server.Modules.Core.Mapping");
        }


    }
}
