using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Server.Modules.Core.Common.Interfaces;
using Server.Modules.Core.Common.Models;
using Server.Modules.Core.Common.Request;
using Server.Modules.Core.Common.Response;

namespace Server.Modules.Core.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {

        private readonly ILogger<Book> _logger;
        private readonly IBookBusiness _bookBusiness;

        public BookController(ILogger<Book> logger, IBookBusiness bookBusiness)
        {

            _logger = logger;
            _bookBusiness = bookBusiness;
            
        }

        [HttpGet]
        public IEnumerable<Book> Get()
        {
            // fix this please in the next release ...
            var books = _bookBusiness.ListBooks();
            _logger.LogInformation("books list {books}", JsonConvert.SerializeObject(books));
            return books;
        }


        [HttpGet]
        [Route("Populate")]
        public virtual BookSaveResponse Populate(BookSaveRequest book)
        {
            try
            {
                _bookBusiness.SaveBook(book.Value);

                return new BookSaveResponse
                {
                    Success = true
                };
            }
            catch (System.Exception ex)
            {

                return new BookSaveResponse
                {
                    Exception = ex
                };
            }
        }



        [HttpGet]
        [Route("Update")]
        public void Update()
        {

            var b = new Book
            {
                Id = 1,
                Title = "book text matter 23"
            };

            
            _bookBusiness.SaveBook(b);
        }
    }
}
