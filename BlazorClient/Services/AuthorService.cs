using System.Net.Http.Json;
using BlazorClient.Models;
namespace BlazorClient.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorModel>> GetAuthors();
        Task<IEnumerable<AuthorModel>> SearchAuthor(int id);
        Task InsertAuthor(AuthorModel author);
        Task EditAuthor( AuthorModel author);
        Task DeleteAuthor(AuthorModel author);
    }
    public class AuthorService : IAuthorService
    {
        private readonly HttpClient httpClient;
        public AuthorService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<AuthorModel>> GetAuthors()
        {
            var response = await httpClient.GetFromJsonAsync<List<AuthorModel>>("/Author");
            return response;
        }
        
        public async Task InsertAuthor(AuthorModel author)
        {
             await httpClient.PostAsJsonAsync<AuthorModel>("/Author", author);
        }
        public async Task<IEnumerable<AuthorModel>> SearchAuthor(int id)
        {
           return await httpClient.GetFromJsonAsync<List<AuthorModel>>("/Author/?id=" + id);
        }

        public async Task EditAuthor(AuthorModel author)
        {
            await httpClient.PutAsJsonAsync<AuthorModel>("/Author", author);
        }

        public async Task DeleteAuthor(AuthorModel author)
        {
            
            await httpClient.DeleteAsync("/Author/?id=" + author.author_id);
        }

    }
}
