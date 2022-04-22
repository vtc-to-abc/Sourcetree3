using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorServer.Data.Models;
namespace BlazorServer.Data
{
    public interface IAuthorBookData
    {
        Task<List<AuthorModel>> GetAuthorByBook(BookModel book);
        Task<List<BookModel>> GetBookByAuthor(AuthorModel auth);
        Task<AuthorBookModel> InsertAuthorBook(AuthorBookModel auth);
        Task<AuthorBookModel> Delete(AuthorBookModel auth);

        Task<AuthorBookModel> SearchAuthorBook(AuthorBookModel auth);
    }
    public class AuthorBookData : IAuthorBookData
    {
        private readonly ISqlDataAccess _db;
        public AuthorBookData(ISqlDataAccess db)
        {
            _db = db;
        }
        public async Task<AuthorBookModel> Delete(AuthorBookModel authb)
        {
            string sqlQuery = "delete from dbo.author_book where author_id = @author_id and  book_id = @book_id";
            var result = await SearchAuthorBook(authb);
            await _db.SaveData(sqlQuery, authb);
            return result;
        }


        public async Task<List<AuthorModel>> GetAuthorByBook(BookModel book)
        {
            int book_id1 = book.book_id;

            string sqlQuery = "select * from dbo.author join dbo.author_book on dbo.author.author_id = dbo.author_book.author_id where book_id = @book_id1";
            var result = await _db.LoadDataList<AuthorModel, dynamic>(sqlQuery, new { book_id1 });
            return result;
        }

        public async Task<List<BookModel>> GetBookByAuthor(AuthorModel auth)
        {
            int author_id1 = auth.author_id;
            string sqlQuery = "select * from dbo.book join dbo.author_book on dbo.book.book_id = dbo.author_book.book_id where author_id = @author_id1";
            var result = await _db.LoadDataList<BookModel, dynamic>(sqlQuery, new { author_id1 });
            return result;
        }

        public async Task<AuthorBookModel> InsertAuthorBook(AuthorBookModel authb)
        {
            string sqlQuery = "insert into dbo.author_book(author_id,book_id) values(@author_id,  @book_id);";
            await _db.SaveData(sqlQuery, authb);
            var result = await SearchAuthorBook(authb);
            return result;
        }

        public async Task<AuthorBookModel> SearchAuthorBook(AuthorBookModel authb)
        {
            int book_id1 = authb.book_id;
            int author_id1 = authb.author_id;
            string sqlQuery = "select * from dbo.author_book where book_id = @book_id1 and author_id = @author_id1";

            var result = await _db.LoadData<AuthorBookModel, dynamic>(sqlQuery, new { book_id1, author_id1 });
            return result;

        }

    }
}
