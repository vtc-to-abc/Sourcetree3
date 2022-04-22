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
            if (booksbyauthor != null)
                return Ok(booksbyauthor); //new JsonResult(books);
            else
                return BadRequest("Bad Request");
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
            if (authorsbybook != null)
                return Ok(authorsbybook); //new JsonResult(books);
            else
                return BadRequest("Bad Request");
        }

        [HttpPost(Name = "NewAuthorBookInsert")]
        public async Task<IActionResult> InsertAuthor([FromBody] AuthorBookModel authorbook)
        {
            if (!ModelState.IsValid) // if the [frombody] book is not bindable
                return BadRequest(ModelState);

            var response = await _db.InsertAuthorBook(authorbook);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{aid:int}/{bid:int}")]
        public async Task<IActionResult> DeleteAuthorBook([FromRoute] int aid, [FromRoute] int bid)
        {
            if (!ModelState.IsValid) // if the [frombody] book is not bindable
                return BadRequest(ModelState);
            var authorbook = new AuthorBookModel()
            {
                author_id = aid, 
                book_id = bid
            };


            var response = await _db.Delete(authorbook);
            return Ok(response);

        }

    }
}
