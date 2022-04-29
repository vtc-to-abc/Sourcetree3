using BlazorServer.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BlazorServer.Data.Services
{
    public interface IAccountData
    {
        Task<List<AccountModel>> GetAccount();
        Task<AccountModel> InsertAccount(AccountModel acc);
        Task<AccountModel> EditAccount(AccountModel acc);
        Task<AccountModel> SearchAccount(AccountModel acc);
        Task<AccountModel> Delete(AccountModel acc);

        // authenticate
        Task<AccountModel> Authenticate(AccountModel acc);
        Task<AccountModel> CheckCorrectAccount(AccountModel acc);
        Task<AccountModel> CheckAdminPageAccessibility(AccountModel acc);
    }
    // t
    public class AccountData : IAccountData
    {
        private readonly ISqlDataAccess _db;
        private IConfiguration _configuration;
        public AccountData(ISqlDataAccess db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        #region user_authentication_and_jwt_token_uses
        /// <summary>
        /// when users sign in with their username and password. the Authenticate will start checking 2 things
        /// 1. if this user existed in database? if not > deny their access request
        /// 2. if true, then check if this user have permission to access to what they are requesting? if not then deny their access request
        /// if true, then generate a unique jwt token(which has specific lifespan) for that specific user.
        /// after the jwt token's lifespan end, refresh a new token for them, or kick them out, make them login again, fuck them
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public async Task<AccountModel> Authenticate(AccountModel acc)
        {
            var RequestingAccount = await SearchAccount(acc);

            var RequestingAccountPermission = await CheckAdminPageAccessibility(RequestingAccount);

            if (RequestingAccount == null)
            {
                return null;
            }

            if (RequestingAccount != null && RequestingAccountPermission == null)
                return null;

            var Token =  GenerateJwtToken(RequestingAccount);
            RequestingAccount.Token = Token;
            return RequestingAccount;
        }
        public string GenerateJwtToken(AccountModel acc)
        {

            var key = _configuration.GetValue<string>("JwtConfig:Key");
            var keyBytes = Encoding.ASCII.GetBytes(key);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, acc.account_username),
                    new Claim(ClaimTypes.Role , acc.account_role_id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        // after that, await attactation
        #endregion 

        public async Task<AccountModel> CheckAdminPageAccessibility(AccountModel acc)
        {
            int account_role_id1;
            if (acc == null) account_role_id1 = 0;
            else account_role_id1 = acc.account_role_id;
            string sqlQuery = @"select * from dbo.account 
                                join dbo.role_permission
                                on dbo.account.account_role_id = dbo.role_permission.account_role_id and permission_id = 1
                                where dbo.account.account_role_id = @account_role_id1";

            var result = await _db.LoadData<AccountModel, dynamic>(sqlQuery, new { account_role_id1 });

            return result;
        }

        public async Task<AccountModel> CheckCorrectAccount(AccountModel acc)
        {
            string sqlQuery = "select * from dbo.account where account_username = @account_username and account_password = @account_password";
            var result = await _db.LoadData<AccountModel, dynamic>(sqlQuery, acc);
            return result;
        }

        public async Task<AccountModel> Delete(AccountModel acc)
        {
            string sqlQuery = "delete from dbo.account where account_username = @account_username";
            var deletedAccount = await SearchAccount(acc);
            await _db.SaveData(sqlQuery, acc);
            return deletedAccount;
        }

        public async Task<AccountModel> EditAccount(AccountModel acc)
        {
            string sqlQuery = "update dbo.account set account_password = @account_password, account_role_id = @account_role_id where account_username = @account_username";
            await _db.SaveData(sqlQuery, acc);

            var editedAccount = await SearchAccount(acc);
            return editedAccount;
        }

        public async Task<List<AccountModel>> GetAccount()
        {
            string sqlQuery = "select * from dbo.account";
            var result = await _db.LoadDataList<AccountModel, dynamic>(sqlQuery, null);
            return result;
        }
        public async Task<AccountModel> InsertAccount(AccountModel acc)
        {
            string sqlQuery = "insert into dbo.account(account_username, account_password, account_role_id) values(@account_username, @account_password, @account_role_id);";
            await _db.SaveData(sqlQuery, acc);

            var insertedAccount = await SearchAccount(acc);
            return insertedAccount;
        }

        public async Task<AccountModel> SearchAccount(AccountModel acc)
        {
            
            string sqlQuery = "select * from dbo.account where account_username = @account_username";
            var result = await _db.LoadData<AccountModel, dynamic>(sqlQuery, acc);
            return result;
        }

    }
}
