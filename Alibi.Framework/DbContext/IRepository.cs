using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Alibi.Framework.DbContext
{
    public interface IRepository<T> where T : class, IDisposable
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void CloseTransaction();
        void CloseSession();
        //T Save(T entity);
        //void Delete(T entity);
        //IQueryable<T> Books { get; }


        T FirstOrDefault();

        void Save(T entity);
        void Save(IEnumerable<T> items);
       
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        void Delete(Expression<Func<T, bool>> predicate);



        int Count();
        int Count(Expression<Func<T, bool>> predicate);



        IList<T> List(Expression<Func<T, bool>> predicate);
        IList<T> List<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool orderByDescending);

        IList<T> List(Expression<Func<T, bool>> predicate, int skip, int take);

        IList<T> List<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool orderByDescending, int skip, int take);













        T FindById(int id);
        T FindById<TV>(TV id);
        IQueryable<T> All();
        T FindBy(Expression<Func<T, bool>> expression);
        IQueryable<T> FilterBy(Expression<Func<T, bool>> expression);

        void Dispose();

    }
}
