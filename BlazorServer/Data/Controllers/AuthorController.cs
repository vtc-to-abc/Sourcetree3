using Microsoft.AspNetCore.Mvc;
using BlazorServer.Data.Models;
namespace BlazorServer.Data.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
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
            return new JsonResult(authors); 
        }

        [HttpPost(Name ="NewAuthorInsert")]
        public async Task InsertAuthor([FromBody]AuthorModel author)
        {
            await _db.InsertAuthor(author);
        }

        [HttpPut(Name ="UpdateAuthor")]
        public async Task UpdateAuthor([FromBody] AuthorModel author)
        {
            await _db.EditAuthor(author);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> AuthorDetail([FromRoute] int id)
        {
            var authorToUpdate = new AuthorModel()
            {
                author_id = id
            };
            authors = await _db.SearchAuthor(authorToUpdate);

            return new JsonResult(authors);
            
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task DeleteAuthor([FromRoute] int id)
        {
            var author = new AuthorModel()
            {
                author_id = id
            };
           await _db.Delete(author);

        }
    }
}
