using Microsoft.AspNetCore.Mvc;
using BlazorServer.Data.Models;
using BlazorServer.Data.Services;
using Microsoft.AspNetCore.Authorization;

namespace BlazorServer.Data.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [Authorize(Policy = "SuperAdmin")]

    public class AuthorController:ControllerBase
    {
        private readonly IAuthorData _db;
        private readonly ILogger<AuthorController>  _logger;
        private List<AuthorModel> authors = new();
        public AuthorController(IAuthorData db, ILogger<AuthorController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "AuthorIndex")]
        public async Task<IActionResult> AuthorIndex()
        {
            authors = await _db.GetAuthor();
            if (authors != null)
                return Ok(authors); //new JsonResult(books);
            else
                return BadRequest("Bad Request");
        }

        [HttpPost(Name ="NewAuthorInsert")]
        public async Task<IActionResult> InsertAuthor([FromBody]AuthorModel author)
        {

            if (!ModelState.IsValid) // if the [frombody] book is not bindable
                return BadRequest(ModelState);

            var response = await _db.InsertAuthor(author);
            return Ok(response);
        }

        [HttpPut(Name ="UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthor([FromBody] AuthorModel author)
        {
            if (!ModelState.IsValid) // if the [frombody] book is not bindable
                return BadRequest(ModelState);

            var response = await _db.EditAuthor(author);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> AuthorDetail([FromQuery] int id)
        {
            var authorToUpdate = new AuthorModel()
            {
                author_id = id
            };
            var author = await _db.SearchAuthor(authorToUpdate);
            if (author != null)
                return Ok(authors); //new JsonResult(books);
            else
                return BadRequest("Bad Request");

        }


        [HttpDelete]
        public async Task<IActionResult> DeleteAuthor([FromQuery] int id)
        {
            if (!ModelState.IsValid) // if the [frombody] book is not bindable
                return BadRequest(ModelState);
            var author = new AuthorModel()
            {
                author_id = id
            };
            var response = await _db.Delete(author);
            return Ok(response);

        }
    }
}
