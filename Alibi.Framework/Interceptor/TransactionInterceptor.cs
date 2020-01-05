using Castle.DynamicProxy;
using NHibernate;
using System.Transactions;

namespace Alibi.Framework.Interceptor
{
    public class TransactionInterceptor : Castle.DynamicProxy.IInterceptor
    {
        private readonly ISession _session = null;

        public TransactionInterceptor(ISession session)
        {
            _session = session;
        }

        public void Intercept(IInvocation invocation)
        {
            using var tx = new TransactionScope();
            invocation.Proceed();
            _session.Flush();
            tx.Complete();
        }
    }
}