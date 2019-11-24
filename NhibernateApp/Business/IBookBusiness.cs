using NhibernateApp.Models;
using System.Collections.Generic;

namespace NhibernateApp.Business
{
    public interface IBookBusiness
    {

        IEnumerable<Book> ListBooks();

        Book SaveBook(Book book);

    }
}
