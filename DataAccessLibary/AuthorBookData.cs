using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibary.Models;
namespace DataAccessLibary
{
    public interface IAuthorBookData
    {
        Task<List<AuthorBookModel>> GetAuthorBook();
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


        public Task<List<AuthorBookModel>> GetAuthorBook()
        {
            string sqlquery = "select * from dbo.author_book";
            var result = _db.LoadData<AuthorBookModel, dynamic>(sqlquery, new { });
            return result;
        }

        public Task InsertAuthorBook(AuthorBookModel authb)
        {
            string sqlquery = "insert into dbo.author_book(author_id,book_id) values(@author_id,  @book_id);";
            return _db.SaveData(sqlquery, authb);
        }

    }
}
