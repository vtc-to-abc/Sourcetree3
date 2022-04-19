using System.Net.Http.Json;
namespace BlazorClient.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAuthors();
    }
    public class AuthorService : IAuthorService
    {
        private readonly HttpClient httpClient;
        public AuthorService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Author>> GetAuthors()
        {
            var response = await httpClient.GetFromJsonAsync<List<Author>>("/Author");
            return response;
        }
        /*public Task<IEnumerable<Author>> GetAuthors()
{
*/
    }
}
