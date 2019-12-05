using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Server.Modules.Core.Common.Interfaces;
using Server.Modules.Core.Common.Models;
using System.Collections.Generic;

namespace Server.Modules.Core.Controllers
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

            var books = _bookBusiness.ListBooks();
            _logger.LogInformation("books list {books}", JsonConvert.SerializeObject(books));
            return books;
        }


        [HttpGet]
        [Route("Populate")]
        public void Populate()
        {
            var b = new Book
            {
                Title = "book text matter"
            };
            _bookBusiness.SaveBook(b);
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
