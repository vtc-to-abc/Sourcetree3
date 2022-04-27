using BlazorServer.Data.Models;

namespace BlazorServer.Data
{
    public interface IPermissionData
    {
        Task<List<PermissionModel>> GetPermission();
        Task<PermissionModel> InsertPermission(PermissionModel per);
        Task<PermissionModel> EditPermission(PermissionModel per);
        Task<PermissionModel> SearchPermission(PermissionModel per);
        Task<PermissionModel> Delete(PermissionModel per);
    }
    // t
    public class PermissionData : IPermissionData
    {
        private readonly ISqlDataAccess _db;
        public PermissionData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<PermissionModel> Delete(PermissionModel per)
        {
            string sqlQuery = "delete from dbo.permission where permission_id = @permission_id";
            var deletedPermission = await SearchPermission(per);
            await _db.SaveData(sqlQuery, per);
            return deletedPermission;
        }

        public async Task<PermissionModel> EditPermission(PermissionModel per)
        {
            string sqlQuery = "update dbo.permission set permission_description = @permission_description where permission_id = @permission_id";
            await _db.SaveData(sqlQuery, per);

            var editedPermission = await SearchPermission(per);
            return editedPermission;
        }

        public async Task<List<PermissionModel>> GetPermission()
        {
            string sqlQuery = "select * from dbo.permission";
            var result = await _db.LoadDataList<PermissionModel, dynamic>(sqlQuery, null);
            return result;
        }
        public async Task<PermissionModel> InsertPermission(PermissionModel per)
        {
            string sqlQuery = "insert into dbo.permission(permission_description) values(@permission_description);";
            await _db.SaveData(sqlQuery, per);

            var insertedPermission = await SearchPermission(per);
            return insertedPermission;
        }

        public async Task<PermissionModel> SearchPermission(PermissionModel per)
        {
            string sqlQuery = "select * from dbo.permission where permission_id = @permission_id";
            var result = await _db.LoadData<PermissionModel, dynamic>(sqlQuery, per);
            return result;
        }
    }
}
