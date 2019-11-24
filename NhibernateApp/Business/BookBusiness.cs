using NHibernate;
using NhibernateApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace NhibernateApp.Business
{
    public class BookBusiness : BaseBusiness<Book>, IBookBusiness
    {

        public BookBusiness(ISession session) : base(session)
        {
        }

        public IEnumerable<Book> ListBooks()
        {
            return Books.ToList();
        }

        public Book SaveBook(Book book)
        {
            try
            {
                BeginTransaction();

                var entity = Save(book);

                Commit();

                CloseTransaction();


                return entity;
            }
            catch (System.Exception)
            {

                return null;
            }


        }
    }
}
