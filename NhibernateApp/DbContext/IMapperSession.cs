using System.Linq;

namespace NhibernateApp.DbContext
{
    public interface IMapperSession<T>
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        void CloseTransaction();
        T Save(T entity);
        void Delete(T entity);
        IQueryable<T> Books { get; }
    }
}
