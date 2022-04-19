using Microsoft.AspNetCore.Mvc;
using BlazorServer.Data.Models;
namespace BlazorServer.Data.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class AuthorBookController : ControllerBase
    {
        private readonly IAuthorBookData _db;
        private readonly ILogger<AuthorController> _logger;
        private List<AuthorModel> authorsbybook = new();
        private List<BookModel> booksbyauthor = new();

        public AuthorBookController(IAuthorBookData db, ILogger<AuthorController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("{aid:int}")]
        [ActionName("GetAllBookByAuthor")]
        public async Task<IActionResult> GetAllBookByAuthor([FromRoute] int aid)
        {
            var author = new AuthorModel()
            {
                author_id = aid
            };
            booksbyauthor = await _db.GetBookByAuthor(author);
            return new JsonResult(booksbyauthor);
        }

        [HttpGet("{bid:int}")]
        [ActionName("GetAllAuthorByBook")]

        public async Task<IActionResult> GetAllAuthorByBook([FromRoute] int bid)
        {
            var book = new BookModel()
            {
                book_id = bid
            };
            authorsbybook = await _db.GetAuthorByBook(book);
            return new JsonResult(authorsbybook);
        }

        [HttpPost(Name = "NewAuthorBookInsert")]
        public async Task InsertAuthor([FromBody] AuthorBookModel authorbook)
        {
            await _db.InsertAuthorBook(authorbook);
        }

        [HttpDelete]
        [Route("{aid:int}/{bid:int}")]
        public async Task DeleteAuthorBook([FromRoute] int aid, [FromRoute] int bid)
        {
            var authorbook = new AuthorBookModel()
            {
                author_id = aid, 
                book_id = bid
            };
            await _db.Delete(authorbook);

        }

    }
}
