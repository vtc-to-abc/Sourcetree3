using BlazorServer.Data.Models;
namespace BlazorServer.Data.Services
{
    public interface IRolePermissionData
    {
        Task<List<PermissionModel>> GetPermissionByRole(RoleModel role);
        Task<List<RoleModel>> GetRoleByPermission(PermissionModel permission);
        Task<RolePermissionModel> InsertRolePermission(RolePermissionModel rolp);
        Task<RolePermissionModel> DeleteRolePermission(RolePermissionModel rolp);
        Task<RolePermissionModel> SearchRolePermission(RolePermissionModel rolp);
    }
    public class RolePermissionData : IRolePermissionData
    {
        private readonly ISqlDataAccess _db;
        public RolePermissionData(ISqlDataAccess db)
        {
            _db = db;
        }
        public async Task<RolePermissionModel> DeleteRolePermission(RolePermissionModel rolp)
        {
            string sqlQuery = "delete from dbo.role_permission where account_role_id = @account_role_id and  permission_id = @permission_id";
            var result = await SearchRolePermission(rolp);
            await _db.SaveData(sqlQuery, rolp);
            return result;
        }

        public async Task<List<PermissionModel>> GetPermissionByRole(RoleModel role)
        {
            int role_id1 = role.account_role_id;

            string sqlQuery = "select * from dbo.permission join dbo.role_permission on dbo.permission.permission_id = dbo.role_permission.permission_id where account_role_id = @role_id1";
            var result = await _db.LoadDataList<PermissionModel, dynamic>(sqlQuery, new { role_id1 });
            return result;
        }

        public async Task<List<RoleModel>> GetRoleByPermission(PermissionModel permission)
        {
            int permission_id1 = permission.permission_id;
            string sqlQuery = "select * from dbo.role join dbo.role_permission on dbo.role.account_role_id = dbo.role_permission.account_role_id where permission_id = @permission_id1";
            var result = await _db.LoadDataList<RoleModel, dynamic>(sqlQuery, new { permission_id1 });
            return result;
        }

        public async Task<RolePermissionModel> InsertRolePermission(RolePermissionModel rolp)
        {
            string sqlQuery = "insert into dbo.role_permission(account_role_id,permission_id) values(@account_role_id,  @permission_id);";
            await _db.SaveData(sqlQuery, rolp);
            var result = await SearchRolePermission(rolp);
            return result;
        }

        public async Task<RolePermissionModel> SearchRolePermission(RolePermissionModel rolp)
        {
            int role_id1 = rolp.account_role_id;
            int permission_id1 = rolp.permission_id;
            string sqlQuery = "select * from dbo.role_permission where account_role_id = @role_id1 and permission_id = @permission_id1";

            var result = await _db.LoadData<RolePermissionModel, dynamic>(sqlQuery, new { role_id1, permission_id1 });
            return result;
        }
    }
}
