using Alibi.Framework.DbContext;
using NHibernate;

namespace Alibi.Framework.Business
{
    public class BaseBusiness<T> : NHibernateMapperSession<T> where T : class
    {
        public BaseBusiness(ISession session) : base(session)
        {
        }

        

    }
}
