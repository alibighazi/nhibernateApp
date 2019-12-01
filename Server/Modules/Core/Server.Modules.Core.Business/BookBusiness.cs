using Alibi.Framework.DbContext;
using Server.Modules.Core.Common.Interfaces;
using Server.Modules.Core.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace Server.Modules.Core.Business
{
    public class BookBusiness : IBookBusiness
    {
        private IMapperSession<Book> _session;
        public BookBusiness(IMapperSession<Book> session)
        {
            _session = session;
        }

        public IEnumerable<Book> ListBooks()
        {
            return _session.Books.ToList();
        }

        public Book SaveBook(Book book)
        {
            try
            {
                _session.BeginTransaction();

                var entity = _session.Save(book);

                _session.Commit();

                _session.CloseTransaction();


                return entity;
            }
            catch (System.Exception ex)
            {

                return null;
            }


        }
    }
}
