using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibary.Models;
namespace DataAccessLibary
{
    public interface IBookData
    {
        Task<List<BookModel>> GetBook();
        Task InsertBook(BookModel auth);
        Task EditBook(BookModel auth);
        Task<List<BookModel>> SearchBook(BookModel auth);
        Task Delete(BookModel auth);
    }
    public class BookData : IBookData
    {
        private readonly ISqlDataAccess _db;
        public BookData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task Delete(BookModel book)
        {
            string sqlquery = "delete from dbo.book where book_id = @book_id";
            return _db.SaveData(sqlquery, book);
        }

        public Task EditBook(BookModel book)
        {
            string sqlquery = @"update dbo.book 
                                set book_title = @book_title,
                                    stored_copies = @stored_copies,
                                    current_rent = @current_rent
                                where book_id = @book_id";

            return _db.SaveData(sqlquery, book);
        }

        public Task<List<BookModel>> GetBook()
        {
            string sqlquery = "select * from dbo.book";
            return _db.LoadData<BookModel, dynamic>(sqlquery, new { });
        }

        public Task InsertBook(BookModel book)
        {
            string sqlquery = @"insert into dbo.book(book_title, stored_copies, current_rent) 
                                values(@book_title, @stored_copies, @current_rent)";

            return _db.SaveData(sqlquery, book);
        }

        public Task<List<BookModel>> SearchBook(BookModel book)
        {
            string sqlquery = "select * from dbo.book where book_id = @book_id";
            return _db.LoadData<BookModel, dynamic>(sqlquery, book);
        }
    }
}
