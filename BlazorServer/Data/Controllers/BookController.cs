using Microsoft.AspNetCore.Mvc;
using BlazorServer.Data.Models;
namespace BlazorServer.Data.Controllers
{
    // there are 4 api passing types: Header, Path, Query, Request Body
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Index()
        {
            books = await _db.GetBook();
            if (books != null)
                return Ok(books); //new JsonResult(books);
            else
                return BadRequest("Bad Request");
        }

        [HttpPost(Name = "NewBookInsert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InsertBook( [FromBody] BookModel book)// request body
        {
            if (!ModelState.IsValid) // if the [frombody] book is not bindable
                return BadRequest(ModelState);
            
            var response = await _db.InsertBook(book);
            return Ok(response); 
        }

        [HttpPut(Name = "UpdateBook")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBook([FromBody] BookModel book)
        {
            if (!ModelState.IsValid) // if the [frombody] book is not bindable
                return BadRequest(ModelState);

            var response = await _db.EditBook(book);
            return Ok(response);
        }

        [HttpGet("{id:int}")] // server route passing
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BookDetail([FromQuery] int id)// path
        {
            var bookToUpdate = new BookModel()
            {
                book_id = id
            };
            var book = await _db.SearchBook(bookToUpdate);
            if (book != null)
                return Ok(book);
            else return BadRequest("Bad Request");

        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBook([FromQuery] int id)
        {
            if (!ModelState.IsValid) // if the [frombody] book is not bindable
                return BadRequest(ModelState);

            var book = new BookModel()
            {
                book_id = id
            };
            var response = await _db.Delete(book);
            return Ok(response);

        }
    }
}
