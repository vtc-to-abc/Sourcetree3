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
        private IHttpService _httpService;
        public BookService(IHttpService httpService)
        {
            this._httpService = httpService;
        }

        public async Task<IEnumerable<BookModel>> GetBooks()
        {
            var response = await _httpService.Get<List<BookModel>>("/Book");
            return response;
        }

        public async Task InsertBook(BookModel Book)
        {
            await _httpService.Post<BookModel>("/Book", Book);
        }
        public async Task<IEnumerable<BookModel>> SearchBook(int id)
        {
            return await _httpService.Get<List<BookModel>>("/Book/?id=" + id); // client route-passing receive
        }

        public async Task EditBook(BookModel Book)
        {
            await _httpService.Put<BookModel>("/Book", Book);
        }

        public async Task DeleteBook(BookModel Book)
        {
            await _httpService.Delete("/Book/?id=" + Book.book_id);
        }

    }
}
