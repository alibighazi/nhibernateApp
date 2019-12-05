using Alibi.Framework.DbContext;
using Server.Modules.Core.Common.Interfaces;
using Server.Modules.Core.Common.Models;
using System;
using System.Collections.Generic;

namespace Server.Modules.Core.Business
{
    public class BookBusiness : IBookBusiness
    {
        private IRepository<Book> _repository;
        public BookBusiness(IRepository<Book> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Book> ListBooks()
        {
            return _repository.All();
        }

        public Book BookById(int id)
        {
            var a = _repository.Count();
            var aa = _repository.List<string>(w => w.Id > 0, ww => ww.Title, true);
            _repository.Delete(x => x.Id == id);
            return _repository.FirstOrDefault();
        }

        public void SaveBook(Book book)
        {
            _repository.Save(book);
        }
    }
}
