using Alibi.Framework.DbContext;
using Alibi.Framework.Validation;
using FluentValidation;
using Server.Modules.Core.Common.Interfaces;
using Server.Modules.Core.Common.Models;
using System;
using System.Collections.Generic;

namespace Server.Modules.Core.Business
{



    //public sealed class SignInModelValidator : Validator<Book>
    //{
    //    public SignInModelValidator()
    //    {
    //        WithMessage("Book is invalid.");
    //        RuleFor(x => x.Title)
    //            .NotEmpty()
    //            .MinimumLength(3)
    //            .Matches(Regexes.Email);
    //    }
    //}


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
            //var validation = new SignInModelValidator().Validate(book);
            //if(validation.Succeeded)
            //{

            //}



            _repository.Save(book);
        }
    }
}
