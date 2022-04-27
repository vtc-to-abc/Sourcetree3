using Microsoft.AspNetCore.Mvc;
using BlazorServer.Data.Models;
using BlazorServer.Data.Services;
namespace BlazorServer.Data.ControllersPermission
{
    // there are 4 api passing types: Header, Path, Query, Request Body
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionData _db;
        private readonly ILogger<PermissionController> _logger;
        private List<PermissionModel> _permissions = new();

        public PermissionController(IPermissionData db, ILogger<PermissionController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "PermissionIndex")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Index()
        {
            _permissions = await _db.GetPermission();
            if (_permissions != null)
                return Ok(_permissions); //new JsonResult(Permissions);
            else
                return BadRequest("Bad Request");
        }

        [HttpPost(Name = "NewPermissionInsert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InsertPermission([FromBody] PermissionModel permission)// request body
        {
            if (!ModelState.IsValid) // if the [frombody] Permission is not bindable
                return BadRequest(ModelState);

            var response = await _db.InsertPermission(permission);
            return Ok(response);
        }

        [HttpPut(Name = "UpdatePermission")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePermission([FromBody] PermissionModel permission)
        {
            if (!ModelState.IsValid) // if the [frombody] Permission is not bindable
                return BadRequest(ModelState);

            var response = await _db.EditPermission(permission);
            return Ok(response);
        }

        [HttpGet("{id}")] // server route passing
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PermissionDetail([FromQuery] int id)// path
        {
            var permissionToUpdate = new PermissionModel()
            {
                permission_id = id
            };
            var permission = await _db.SearchPermission(permissionToUpdate);
            if (permission != null)
                return Ok(permission);
            else return BadRequest("Bad Request");

        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePermission([FromQuery] int id)
        {
            if (!ModelState.IsValid) // if the [frombody] Permission is not bindable
                return BadRequest(ModelState);

            var permission = new PermissionModel()
            {
                permission_id = id
            };
            var response = await _db.Delete(permission);
            return Ok(response);

        }
    }
}
