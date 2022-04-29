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
        private IHttpService _httpService;
        public AuthorService(IHttpService httpService)
        {
            this._httpService = httpService;
        }

        public async Task<IEnumerable<AuthorModel>> GetAuthors()
        {
            var response = await _httpService.Get<List<AuthorModel>>("/Author");
            return response;
        }
        
        public async Task InsertAuthor(AuthorModel author)
        {
             await _httpService.Post<AuthorModel>("/Author", author);
        }
        public async Task<IEnumerable<AuthorModel>> SearchAuthor(int id)
        {
           return await _httpService.Get<List<AuthorModel>>("/Author/?id=" + id);
        }

        public async Task EditAuthor(AuthorModel author)
        {
            await _httpService.Put<AuthorModel>("/Author", author);
        }

        public async Task DeleteAuthor(AuthorModel author)
        {
            
            await _httpService.Delete("/Author/?id=" + author.author_id);
        }

    }
}
