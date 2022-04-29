using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlazorServer.Data.Models;
using BlazorServer.Data.Services;
namespace BlazorServer.Data.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AdminUserController : Controller
    {
        private readonly ILogger<AdminUserController> _logger;
        private readonly IAccountData _db;

        public AdminUserController(IAccountData db, ILogger<AdminUserController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody]AccountModel acc)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _db.Authenticate(acc);

            if (result == null)
            {
                return Unauthorized(new {message="Account is incorrect or dont have access permission"});
            }
            return Ok(result);
            
        }

        //[HttpGet("denied")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public IActionResult Denied()
        //{
        //    return View();
        //}

        //[Authorize]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync();
        //    return Redirect("/");
        //}

    }
}
