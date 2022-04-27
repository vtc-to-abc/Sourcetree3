using Microsoft.AspNetCore.Mvc;
using BlazorServer.Data.Models;
using BlazorServer.Data.Services;
namespace BlazorServer.Data.Controllers
{
    // there are 4 api passing types: Header, Path, Query, Request Body
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountData _db;
        private readonly ILogger<AccountController> _logger;
        private List<AccountModel> _accounts = new();

        public AccountController(IAccountData db, ILogger<AccountController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "AccountIndex")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Index()
        {
            _accounts = await _db.GetAccount();
            if (_accounts != null)
                return Ok(_accounts); //new JsonResult(Accounts);
            else
                return BadRequest("Bad Request");
        }

        [HttpPost(Name = "NewAccountInsert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InsertAccount([FromBody] AccountModel account)// request body
        {
            if (!ModelState.IsValid) // if the [frombody] Account is not bindable
                return BadRequest(ModelState);

            var response = await _db.InsertAccount(account);
            return Ok(response);
        }

        [HttpPut(Name = "UpdateAccount")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountModel account)
        {
            if (!ModelState.IsValid) // if the [frombody] Account is not bindable
                return BadRequest(ModelState);

            var response = await _db.EditAccount(account);
            return Ok(response);
        }

        [HttpGet("{username}")] // server route passing
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AccountDetail([FromQuery] string username)// path
        {
            var AccountToUpdate = new AccountModel()
            {
                account_username = username
            };
            var account = await _db.SearchAccount(AccountToUpdate);
            if (account != null)
                return Ok(account);
            else return BadRequest("Bad Request");

        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAccount([FromQuery] string username)
        {
            if (!ModelState.IsValid) // if the [frombody] Account is not bindable
                return BadRequest(ModelState);

            var Account = new AccountModel()
            {
                account_username = username
            };
            var response = await _db.Delete(Account);
            return Ok(response);

        }
    }
}
