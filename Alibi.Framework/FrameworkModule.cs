using Alibi.Framework.Business;
using Alibi.Framework.Controllers;
using Alibi.Framework.DbContext;
using Alibi.Framework.Interceptor;
using Alibi.Framework.Models;
using Autofac;
using Microsoft.AspNetCore.Http;

namespace Alibi.Framework
{
    public class FrameworkModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<AuthenticationBusiness>()
                .As<IAuthenticationBusiness>();
           
            builder.Register(c => new HttpContextAccessor())
                .As<IHttpContextAccessor>().SingleInstance();

            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();


            builder.Register(c => new TransactionInterceptor(c.Resolve<NHibernate.ISession>()));

            builder.Register(c => new AuthenticatedUserInterceptor(c.Resolve<IHttpContextAccessor>(), c.Resolve<IRepository<UserIdentityModel>>()));



            builder.RegisterType<AuthenticationController>()
                .PropertiesAutowired();
        }


    }
}
