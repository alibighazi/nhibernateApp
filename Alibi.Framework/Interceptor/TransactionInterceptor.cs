using Castle.DynamicProxy;

namespace Alibi.Framework.Interceptor
{
    public class TransactionInterceptor : IInterceptor
    {

        public TransactionInterceptor()
        {

        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}
