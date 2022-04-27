using Microsoft.AspNetCore.Mvc;
using BlazorServer.Data.Models;
using BlazorServer.Data.Services;
namespace BlazorServer.Data.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class RolePermissionController : ControllerBase
    {
        private readonly IRolePermissionData _db;
        private readonly ILogger<RolePermissionController> _logger;
        private List<PermissionModel> _permissionByRole = new();
        private List<RoleModel> _roleByPermission = new();

        public RolePermissionController(IRolePermissionData db, ILogger<RolePermissionController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("{pid}")]
        [ActionName("GetAllRoleByPermission")]
        public async Task<IActionResult> GetAllRoleByPermission([FromRoute] int pid)
        {
            var Permission = new PermissionModel()
            {
                permission_id = pid
            };
            _roleByPermission = await _db.GetRoleByPermission(Permission);
            if (_roleByPermission != null)
                return Ok(_roleByPermission); //new JsonResult(Roles);
            else
                return BadRequest("Bad Request");
        }

        [HttpGet("{rid}")]
        [ActionName("GetAllPermissionByRole")]

        public async Task<IActionResult> GetAllPermissionByRole([FromRoute] int rid)
        {
            var Role = new RoleModel()
            {
                account_role_id = rid
            };
            _permissionByRole = await _db.GetPermissionByRole(Role);
            if (_permissionByRole != null)
                return Ok(_permissionByRole); //new JsonResult(Roles);
            else
                return BadRequest("Bad Request");
        }

        [HttpPost(Name = "NewPermissionRoleInsert")]
        public async Task<IActionResult> InsertPermission([FromBody] RolePermissionModel rolp)
        {
            if (!ModelState.IsValid) // if the [frombody] Role is not bindable
                return BadRequest(ModelState);

            var response = await _db.InsertRolePermission(rolp);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{rid}/{pid}")]
        public async Task<IActionResult> DeletePermissionRole([FromRoute] int rid, [FromRoute] int pid)
        {
            if (!ModelState.IsValid) // if the [frombody] Role is not bindable
                return BadRequest(ModelState);
            var rolePermission = new RolePermissionModel()
            {
                account_role_id = rid,
                permission_id = pid
            };


            var response = await _db.DeleteRolePermission(rolePermission);
            return Ok(response);

        }

    }
}
