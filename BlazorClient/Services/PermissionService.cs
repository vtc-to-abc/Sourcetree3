using System.Net.Http.Json;
using BlazorClient.Models;
namespace BlazorClient.Services
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionModel>> GetPermissions();
        Task<IEnumerable<PermissionModel>> SearchPermission(int id);
        Task InsertPermission(PermissionModel permission);
        Task EditPermission(PermissionModel permission);
        Task DeletePermission(PermissionModel permission);
    }
    public class PermissionService : IPermissionService
    {
        private readonly HttpClient httpClient;
        public PermissionService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<PermissionModel>> GetPermissions()
        {
            var response = await httpClient.GetFromJsonAsync<List<PermissionModel>>("/Permission");
            return response;
        }

        public async Task InsertPermission(PermissionModel permission)
        {
            await httpClient.PostAsJsonAsync<PermissionModel>("/Permission", permission);
        }
        public async Task<IEnumerable<PermissionModel>> SearchPermission(int id)
        {
            return await httpClient.GetFromJsonAsync<List<PermissionModel>>("/Permission/?id=" + id); // client route-passing receive
        }

        public async Task EditPermission(PermissionModel permission)
        {
            await httpClient.PutAsJsonAsync<PermissionModel>("/Permission", permission);
        }

        public async Task DeletePermission(PermissionModel permission)
        {
            await httpClient.DeleteAsync("/Permission/?id=" + permission.permission_id);
        }

    }
}
