using System.Net.Http.Json;
using BlazorClient.Models;
namespace BlazorClient.Services
{
    public interface IRolePermissionService
    {
        Task<IEnumerable<PermissionModel>> GetPermissionByRole(PermissionModel per);
        Task InsertRolePermission(RolePermissionModel rolp);
        Task DeleteRolePermission(RolePermissionModel rolp);
    }
    public class RolePermissionService : IRolePermissionService
    {
        private readonly HttpClient httpClient;
        public RolePermissionService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<PermissionModel>> GetPermissionByRole(PermissionModel per)
        {
            var response = await httpClient.GetFromJsonAsync<List<PermissionModel>>("/RolePermission/GetAllPermissionByRole/" + per.permission_id);
            return response;
        }

        public async Task InsertRolePermission(RolePermissionModel rolp)
        {
            await httpClient.PostAsJsonAsync<RolePermissionModel>("/RolePermission", rolp);
        }
        public async Task DeleteRolePermission(RolePermissionModel rolp)
        {
            await httpClient.DeleteAsync("/RolePermission/" + rolp.permission_id +"/"+ rolp.account_role_id);
        }

    }
}
