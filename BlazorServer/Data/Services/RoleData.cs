using BlazorServer.Data.Models;

namespace BlazorServer.Data.Services
{
    public interface IRoleData
    {
        Task<List<RoleModel>> GetRole();
        Task<RoleModel> InsertRole(RoleModel role);
        Task<RoleModel> EditRole(RoleModel role);
        Task<RoleModel> SearchRole(RoleModel role);
        Task<RoleModel> Delete(RoleModel role);
    }
    // t
    public class RoleData : IRoleData
    {
        private readonly ISqlDataAccess _db;
        public RoleData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<RoleModel> Delete(RoleModel role)
        {
            string sqlQuery = "delete from dbo.account_role where account_role_id = @account_role_id";
            var deletedRole = await SearchRole(role);
            await _db.SaveData(sqlQuery, role);
            return deletedRole;
        }

        public async Task<RoleModel> EditRole(RoleModel role)
        {
            string sqlQuery = "update dbo.account_role set account_role_name = @account_role_name where account_role_id = @account_role_id";
            await _db.SaveData(sqlQuery, role);

            var editedRole = await SearchRole(role);
            return editedRole;
        }

        public async Task<List<RoleModel>> GetRole()
        {
            string sqlQuery = "select * from dbo.account_role";
            var result = await _db.LoadDataList<RoleModel, dynamic>(sqlQuery, null);
            return result;
        }
        public async Task<RoleModel> InsertRole(RoleModel role)
        {
            string sqlQuery = "insert into dbo.account_role(account_role_name) values(@account_role_name);";
            await _db.SaveData(sqlQuery, role);

            var insertedRole = await SearchRole(role);
            return insertedRole;
        }

        public async Task<RoleModel> SearchRole(RoleModel role)
        {
            string sqlQuery = "select * from dbo.account_role where account_role_id = @account_role_id";
            var result = await _db.LoadData<RoleModel, dynamic>(sqlQuery, role);
            return result;
        }
    }
}
