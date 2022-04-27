using System.Net.Http.Json;
using BlazorClient.Models;
namespace BlazorClient.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleModel>> GetRoles();
        Task<IEnumerable<RoleModel>> SearchRole(int id);
        Task InsertRole(RoleModel role);
        Task EditRole(RoleModel role);
        Task DeleteRole(RoleModel role);
    }
    public class RoleService : IRoleService
    {
        private readonly HttpClient httpClient;
        public RoleService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<RoleModel>> GetRoles()
        {
            var response = await httpClient.GetFromJsonAsync<List<RoleModel>>("/Role");
            return response;
        }

        public async Task InsertRole(RoleModel role)
        {
            await httpClient.PostAsJsonAsync<RoleModel>("/Role", role);
        }
        public async Task<IEnumerable<RoleModel>> SearchRole(int id)
        {
            return await httpClient.GetFromJsonAsync<List<RoleModel>>("/Role/?id=" + id); // client route-passing receive
        }

        public async Task EditRole(RoleModel role)
        {
            await httpClient.PutAsJsonAsync<RoleModel>("/Role", role);
        }

        public async Task DeleteRole(RoleModel role)
        {
            await httpClient.DeleteAsync("/Role/?id=" + role.account_role_id);
        }

    }
}
