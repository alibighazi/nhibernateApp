using Castle.DynamicProxy;
using NHibernate;
using System.Transactions;

namespace Alibi.Framework.Interceptor
{
    public class TransactionInterceptor : Castle.DynamicProxy.IInterceptor
    {


        private ISession _session = null;

        public TransactionInterceptor(ISession session)
        {
            _session = session;
        }

        public void Intercept(IInvocation invocation)
        {


            using TransactionScope tx = new TransactionScope();
            try
            {
                invocation.Proceed();
                _session.Flush();
                tx.Complete();
            }
            catch
            {
            }

        }
    }
}