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
        Task<AuthorModel> InsertAuthor(AuthorModel auth);
        Task<AuthorModel> EditAuthor(AuthorModel auth);
        Task<AuthorModel> SearchAuthor(AuthorModel auth);
        Task<AuthorModel> Delete(AuthorModel auth);
    }
    // this shit is basically repository
    public class AuthorData : IAuthorData
    {
        private readonly ISqlDataAccess _db;
        public AuthorData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<AuthorModel> Delete(AuthorModel auth)
        {
            string sqlQuery = "delete from dbo.author where author_id = @author_id";
            var deletedAuthor = await SearchAuthor(auth);
            await _db.SaveData(sqlQuery, auth);
            return deletedAuthor;
        }

        public async Task<AuthorModel> EditAuthor(AuthorModel auth)
        {
            string sqlQuery = "update dbo.author set pseudonym = @pseudonym where author_id = @author_id";
            await _db.SaveData(sqlQuery, auth);

            var editedAuthor = await SearchAuthor(auth);
            return editedAuthor;
        }

        public async Task<List<AuthorModel>> GetAuthor()
        {
            string sqlQuery = "select * from dbo.author";
            var result = await _db.LoadDataList<AuthorModel, dynamic>(sqlQuery, null);
            return result;
        }
        public async Task<AuthorModel> InsertAuthor(AuthorModel auth)
        {
            string sqlQuery = "insert into dbo.author(pseudonym) values(@pseudonym);";
            await _db.SaveData(sqlQuery, auth);

            var insertedAuthor = await SearchAuthor(auth);
            return insertedAuthor;
        }

        public async Task<AuthorModel> SearchAuthor(AuthorModel auth)
        {
            string sqlQuery = "select * from dbo.author where author_id = @author_id";
            var result = await _db.LoadData<AuthorModel, dynamic>(sqlQuery,  auth);
            return result;
        }
    }
}
