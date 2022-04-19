using Microsoft.AspNetCore.Mvc;
using BlazorServer.Data.Models;
namespace BlazorServer.Data.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class BookController : ControllerBase
    {
        private readonly IBookData _db;
        private readonly ILogger<BookController> _logger;
        private List<BookModel> books = new();

        public BookController(IBookData db, ILogger<BookController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name ="BookIndex")]
        public async Task<IActionResult> Index()
        {
            books = await _db.GetBook();
            return new JsonResult(books);
        }

        [HttpPost(Name = "NewBookInsert")]
        public async Task InsertAuthor([FromBody] BookModel book)
        {
            await _db.InsertBook(book);
        }

        [HttpPut(Name = "UpdateBook")]
        public async Task UpdateAuthor([FromBody] BookModel book)
        {
            await _db.EditBook(book);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> AuthorDetail([FromRoute] int id)
        {
            var bookToUpdate = new BookModel()
            {
                book_id = id
            };
            books = await _db.SearchBook(bookToUpdate);

            return new JsonResult(books);

        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task DeleteAuthor([FromRoute] int id)
        {
            var book = new BookModel()
            {
                book_id = id
            };
            await _db.Delete(book);

        }
    }
}
