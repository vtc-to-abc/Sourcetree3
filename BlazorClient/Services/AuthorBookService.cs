using System.Net.Http.Json;
using BlazorClient.Models;
namespace BlazorClient.Services
{
    public interface IAuthorBookService
    {
        Task<IEnumerable<AuthorModel>> GetAuthorsByBook(BookModel book);
        Task InsertAuthorBook(AuthorBookModel Book);
        Task DeleteAuthorBook(AuthorBookModel Book);
    }
    public class AuthorBookService : IAuthorBookService
    {
        private readonly HttpClient httpClient;
        public AuthorBookService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<BookModel>> GetBooks()
        {
            var response = await httpClient.GetFromJsonAsync<List<BookModel>>("/Book");
            return response;
        }
        public async Task<IEnumerable<AuthorModel>> GetAuthorsByBook(BookModel book)
        {
            var response = await httpClient.GetFromJsonAsync<List<AuthorModel>>("/AuthorBook/GetAllAuthorByBook/"+ book.book_id);
            return response;
        }

        public async Task InsertAuthorBook(AuthorBookModel AuthorBook)
        {
            await httpClient.PostAsJsonAsync<AuthorBookModel>("/AuthorBook", AuthorBook);
        }
        public async Task DeleteAuthorBook(AuthorBookModel AuthorBook)
        {
            await httpClient.DeleteAsync("/AuthorBook/" + AuthorBook.book_id);
        }

    }
}
