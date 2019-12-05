using Alibi.Framework.Business;
using Alibi.Framework.DbContext;
using Alibi.Framework.Interceptor;
using Autofac;

namespace Alibi.Framework
{
    public class FrameworkModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<AuthenticationBusiness>().As<IAuthenticationBusiness>();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            builder.Register(c => new TransactionInterceptor());


        }


    }
}
