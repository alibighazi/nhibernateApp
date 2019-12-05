using Server.Modules.Core.Common.Models;
using System.Collections.Generic;

namespace Server.Modules.Core.Common.Interfaces
{
    public interface IBookBusiness
    {

        IEnumerable<Book> ListBooks();

        void SaveBook(Book book);

    }
}
