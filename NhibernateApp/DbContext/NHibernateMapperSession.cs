using NHibernate;
using NhibernateApp.Models;
using System.Linq;

namespace NhibernateApp.DbContext
{
    public class NHibernateMapperSession<T> : IMapperSession<T>
    {

        private readonly ISession _session;
        private ITransaction _transaction;

        public NHibernateMapperSession(ISession session)
        {
            _session = session;
        }

        public IQueryable<T> Books => _session.Query<T>();

        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.RollbackAsync();
        }

        public void CloseTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public virtual T Save(T entity)
        {
            _session.SaveOrUpdate(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _session.Delete(entity);
        }

    }
}
