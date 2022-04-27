using Microsoft.AspNetCore.Mvc;
using BlazorServer.Data.Models;
using BlazorServer.Data.Services;
namespace BlazorServer.Data.Controllers
{
    // there are 4 api passing types: Header, Path, Query, Request Body
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleData _db;
        private readonly ILogger<RoleController> _logger;
        private List<RoleModel> _roles = new();

        public RoleController(IRoleData db, ILogger<RoleController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "RoleIndex")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Index()
        {
            _roles = await _db.GetRole();
            if (_roles != null)
                return Ok(_roles); //new JsonResult(Roles);
            else
                return BadRequest("Bad Request");
        }

        [HttpPost(Name = "NewRoleInsert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InsertRole([FromBody] RoleModel role)// request body
        {
            if (!ModelState.IsValid) // if the [frombody] Role is not bindable
                return BadRequest(ModelState);

            var response = await _db.InsertRole(role);
            return Ok(response);
        }

        [HttpPut(Name = "UpdateRole")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRole([FromBody] RoleModel role)
        {
            if (!ModelState.IsValid) // if the [frombody] Role is not bindable
                return BadRequest(ModelState);

            var response = await _db.EditRole(role);
            return Ok(response);
        }

        [HttpGet("{id}")] // server route passing
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RoleDetail([FromQuery] int id)// path
        {
            var RoleToUpdate = new RoleModel()
            {
                account_role_id = id
            };
            var role = await _db.SearchRole(RoleToUpdate);
            if (role != null)
                return Ok(role);
            else return BadRequest("Bad Request");

        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteRole([FromQuery] int id)
        {
            if (!ModelState.IsValid) // if the [frombody] Role is not bindable
                return BadRequest(ModelState);

            var role = new RoleModel()
            {
                account_role_id = id
            };
            var response = await _db.Delete(role);
            return Ok(response);

        }
    }
}
