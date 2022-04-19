using BlazorServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorServer.Data
{
    public interface IAuthorData
    {
        Task<List<AuthorModel>> GetAuthor();
        Task InsertAuthor(AuthorModel auth);
        Task EditAuthor(AuthorModel auth);
        Task<List<AuthorModel>> SearchAuthor(AuthorModel auth);
        Task Delete(AuthorModel auth);
    }
    // this shit is basically repository
    public class AuthorData : IAuthorData
    {
        private readonly ISqlDataAccess _db;
        public AuthorData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task Delete(AuthorModel auth)
        {
            string sqlquery = "delete from dbo.author where author_id = @author_id";
            return _db.SaveData(sqlquery, auth);
        }

        public Task EditAuthor(AuthorModel auth)
        {
            string sqlquery = "update dbo.author set pseudonym = @pseudonym where author_id = @author_id";
            return _db.SaveData(sqlquery, auth);
        }

        public Task<List<AuthorModel>> GetAuthor()
        {
            string sqlquery = "select * from dbo.author";
            var result = _db.LoadData<AuthorModel, dynamic>(sqlquery, new { });
            return result;
        }
        public Task InsertAuthor(AuthorModel auth)
        {
            string sqlquery = "insert into dbo.author(pseudonym) values(@pseudonym);";
            return _db.SaveData(sqlquery, auth);
        }

        public Task<List<AuthorModel>> SearchAuthor(AuthorModel auth)
        {
            string sqlquery = "select * from dbo.author where author_id = @author_id";
            var result =  _db.LoadData<AuthorModel, dynamic>(sqlquery,  auth);
            return result;
        }
    }
}
