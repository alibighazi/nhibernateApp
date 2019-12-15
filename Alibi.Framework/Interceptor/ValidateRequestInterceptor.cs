using Castle.DynamicProxy;

namespace Alibi.Framework.Interceptor
{
    class ValidateRequestInterceptor : Castle.DynamicProxy.IInterceptor
    {

        public ValidateRequestInterceptor()
        {

        }
        public void Intercept(IInvocation invocation)
        {

            //inv
            invocation.Proceed();
        
        
        }
    }
}
