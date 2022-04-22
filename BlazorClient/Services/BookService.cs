using System.Net.Http.Json;
using BlazorClient.Models;
using Newtonsoft.Json;
namespace BlazorClient.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookModel>> GetBooks();
        Task<IEnumerable<BookModel>> SearchBook(int id);
        Task InsertBook(BookModel Book);
        Task EditBook(BookModel Book);
        Task DeleteBook(BookModel Book);
    }
    public class BookService : IBookService
    {
        private readonly HttpClient httpClient;
        public BookService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<BookModel>> GetBooks()
        {
            var response = await httpClient.GetFromJsonAsync<List<BookModel>>("/Book");
            return response;
        }

        public async Task InsertBook(BookModel Book)
        {
            await httpClient.PostAsJsonAsync<BookModel>("/Book", Book);
        }
        public async Task<IEnumerable<BookModel>> SearchBook(int id)
        {
            return await httpClient.GetFromJsonAsync<List<BookModel>>("/Book/?id=" + id); // client route-passing receive
        }

        public async Task EditBook(BookModel Book)
        {
            await httpClient.PutAsJsonAsync<BookModel>("/Book", Book);
        }

        public async Task DeleteBook(BookModel Book)
        {
            await httpClient.DeleteAsync("/Book/?id=" + Book.book_id);
        }

    }
}
