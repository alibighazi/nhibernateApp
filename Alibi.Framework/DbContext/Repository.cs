using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Alibi.Framework.DbContext
{


    public static class NhibernateExtension
    {

        public static IQueryable<T> Paging<T>(this IQueryable<T> query, int skip, int take)
        {
            return query.Skip(skip).Take(take);
        }

        public static IQueryable<T> Ordering<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> field, string direction)
        {
            return direction == "desc" ? query.OrderByDescending(field) : query.OrderBy(field);
        }

    }

    public class Repository<T> : IRepository<T> where T : class, IDisposable
    {

        private readonly ISession _session = null;
        private readonly ITransaction _transaction = null;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _accessor;


        public Repository(ISession session, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
        {
            _session = session;
            _accessor = accessor;
            var q = _accessor.HttpContext.Request.Body;
        }

        #region IRepository Members

        #region save
        public void Save(T entity)
        {
            _session.SaveOrUpdate(entity);
            _session.Flush();

            DetachedCriteria.For<T>().Add(Restrictions.Eq("id", 1));
        }
        public void Save(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                _session.Save(entity);
            }
            _session.Flush();
        }
        #endregion

        #region delete
        public void Delete(T entity)
        {
            _session.Delete(entity);
            _session.Flush();
        }
        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var result = _session.Query<T>().Where(predicate);
            foreach (var o in result)
            {
                _session.Delete(o);
            }
            _session.Flush();
        }
        public void Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _session.Delete(entity);
            }
            _session.Flush();
        }
        #endregion

        #region List

        public IList<T> List()
        {
            return _session.Query<T>().ToList();
        }

        public IList<T> List(Expression<Func<T, bool>> predicate)
        {
            return _session.Query<T>().Where(predicate).ToList();
        }
        public IList<T> List<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool orderByDescending)
        {
            var query = _session.Query<T>().Where(predicate);
            query = orderByDescending ? query.OrderByDescending(order) : query.OrderBy(order);
            return query.ToList();
        }
        public IList<T> List(Expression<Func<T, bool>> predicate, int skip, int take)
        {
            var query = _session.Query<T>().Where(predicate);
            if (skip > 0)
                query = query.Skip(skip);
            return query.Take(take).ToList();
        }
        public IList<T> List<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool orderByDescending, int skip, int take)
        {
            var query = _session.Query<T>().Where(predicate);
            query = orderByDescending ? query.OrderByDescending(order) : query.OrderBy(order);
            if (skip > 0)
                query = query.Skip(skip);
            return query.Take(take).ToList();
        }
        #endregion

        #region count
        public int Count()
        {
            return _session.Query<T>().Count();
        }
        public int Count(Expression<Func<T, bool>> predicate)
        {
            return _session.Query<T>().Count(predicate);
        }
        #endregion

        public T FindById(int id)
        {
            _session.CacheMode = CacheMode.Normal;
            return _session.Get<T>(id);
        }

        public T FindById<TV>(TV id)
        {
            return _session.Get<T>(id);
        }

        public IQueryable<T> All()
        {
            return _session.Query<T>();
        }

        public T FirstOrDefault()
        {
            return _session.Query<T>().WithOptions(x => x.SetCacheable(true)).FirstOrDefault();
        }

        public T FindBy(Expression<Func<T, bool>> expression)
        {
            return FilterBy(expression).FirstOrDefault();
        }

        public IQueryable<T> FilterBy(Expression<Func<T, bool>> expression)
        {
            return All().Where(expression).AsQueryable();
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            if (_transaction != null)
            {
                // Commit transaction by default, unless user explicitly rolls it back.
                // To rollback transaction by default, unless user explicitly commits,                
                // comment out the line below.
            }

            if (_session == null) return;
            _session.Flush(); // commit session transactions
            CloseSession();
        }

        private void CloseSession()
        {
            _session.Close();
        }

        #endregion





        public IList<TT> GetUsers<TT>()
        {
            return _session.CreateSQLQuery("exec SelectUserById :userId")
                .SetResultTransformer(Transformers.AliasToBean<TT>())
                .SetParameter("userId", 1).List<TT>().ToList();
        }

    }
}
