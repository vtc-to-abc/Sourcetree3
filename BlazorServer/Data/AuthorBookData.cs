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
        Task InsertAuthorBook(AuthorBookModel auth);
        Task Delete(AuthorBookModel auth);
    }
    public class AuthorBookData : IAuthorBookData
    {
        private readonly ISqlDataAccess _db;
        public AuthorBookData(ISqlDataAccess db)
        {
            _db = db;
        }
        public Task Delete(AuthorBookModel authb)
        {
            string sqlquery = "delete from dbo.author_book where author_id = @author_id and  book_id = @book_id";
            return _db.SaveData(sqlquery, authb);
        }


        public Task<List<AuthorModel>> GetAuthorByBook(BookModel book)
        {
            int book_id1 = book.book_id;

            string sqlquery = "select * from dbo.author join dbo.author_book on dbo.author.author_id = dbo.author_book.author_id where book_id = @book_id1";
            var result = _db.LoadData<AuthorModel, dynamic>(sqlquery, new { book_id1 });
            return result;
        }

        public Task<List<BookModel>> GetBookByAuthor(AuthorModel auth)
        {
            int author_id1 = auth.author_id;
            string sqlquery = "select * from dbo.book join dbo.author_book on dbo.book.book_id = dbo.author_book.book_id where author_id = @author_id1";
            var result = _db.LoadData<BookModel, dynamic>(sqlquery, new { author_id1 });
            return result;
        }

        public Task InsertAuthorBook(AuthorBookModel authb)
        {
            string sqlquery = "insert into dbo.author_book(author_id,book_id) values(@author_id,  @book_id);";
            return _db.SaveData(sqlquery, authb);
        }

    }
}
