using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;

namespace Alibi.Framework.Interceptor
{
    public class AuthenticatedUserInterceptor : Castle.DynamicProxy.IInterceptor
    {

        private IHttpContextAccessor _accessor;

        public AuthenticatedUserInterceptor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }



        public void Intercept(IInvocation invocation)
        {


           var ipAddr =  _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

            invocation.Proceed();
        }
    }
}