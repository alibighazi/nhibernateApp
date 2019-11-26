using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NhibernateApp.Business;
using NhibernateApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NhibernateApp.Controllers
{
    [ApiController]
    //[Authorize]
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
        public Task<IEnumerable<Book>> Get()
        {

            var books = _bookBusiness.ListBooks();
            _logger.LogInformation("books list {books}", JsonConvert.SerializeObject(books));
            return Task.FromResult<IEnumerable<Book>>(books);
        }


        [HttpGet]
        [Route("Populate")]
        public Book Populate()
        {

            var b = new Book
            {
                Title = "book text matter"
            };

            
            return _bookBusiness.SaveBook(b);
        }



        [HttpGet]
        [Route("Update")]
        public Book Update()
        {

            var b = new Book
            {
                Id = 1,
                Title = "book text matter 23"
            };

            
            return _bookBusiness.SaveBook(b);
        }
    }
}
