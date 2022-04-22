using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorServer.Data.Models;
namespace BlazorServer.Data
{
    public interface IBookData
    {
        Task<List<BookModel>> GetBook();
        Task<BookModel> InsertBook(BookModel auth);
        Task<BookModel> EditBook(BookModel auth);
        Task<BookModel> SearchBook(BookModel auth);
        Task<BookModel> Delete(BookModel auth);
    }
    public class BookData : IBookData
    {
        private readonly ISqlDataAccess _db;
        public BookData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<BookModel> Delete(BookModel book)
        {
            string sqlQuery = "delete from dbo.book where book_id = @book_id";
            var deletedRecord = await SearchBook(book);
            await _db.SaveData(sqlQuery, book);
            return deletedRecord;


        }

        public async Task<BookModel> EditBook(BookModel book)
        {
            string editsqlQuery = @"update dbo.book 
                                set book_title = @book_title,
                                    stored_copies = @stored_copies,
                                    current_rent = @current_rent
                                where book_id = @book_id";

            await _db.SaveData(editsqlQuery, book);

            var editedRecord = await SearchBook(book);
            return editedRecord;
        }

        public async Task<List<BookModel>> GetBook()
        {
            string sqlQuery = "select * from dbo.book";
            var result = await _db.LoadDataList<BookModel, dynamic>(sqlQuery, null);
            return result;
        }

        public async Task<BookModel> InsertBook(BookModel book)
        {
            string sqlQuery = @"insert into dbo.book(book_title, stored_copies, current_rent) 
                                values(@book_title, @stored_copies, @current_rent)";
            await _db.SaveData(sqlQuery, book);

            var insertedRecord = await SearchBook(book);
            return insertedRecord;

        }

        public async Task<BookModel> SearchBook(BookModel book)
        {
            string sqlQuery = "select * from dbo.book where book_id = @book_id";
            var result = await _db.LoadData<BookModel, dynamic>(sqlQuery, book);
            return result;
        }
    }
}
