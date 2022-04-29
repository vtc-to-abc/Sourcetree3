using System.Net.Http.Json;
using BlazorClient.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Services
{
    public interface IAccountService
    {
        AccountModel Account { get; }
        Task Initialize();
        Task Login(AccountModel acc);
        Task<IEnumerable<AccountModel>> GetAccounts();
        Task<IEnumerable<AccountModel>> SearchAccount(string username);
        Task InsertAccount(AccountModel account);
        Task EditAccount(AccountModel account);
        Task DeleteAccount(AccountModel account);
    }
    public class AccountService : IAccountService
    {
        private  IHttpService _httpService;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;
        private string _userKey = "user";
        public AccountService(IHttpService httpService, NavigationManager navigationManager, ILocalStorageService localStorageService)
        {
            _httpService = httpService;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }
        public AccountModel Account { get; private set; }

        public async Task Initialize()
        {

            Account = await _localStorageService.GetItem<AccountModel>(_userKey);
        }
        public async Task Login(AccountModel acc)
        {
            
            Account = await _httpService.Post<AccountModel>("/AdminUser/login", acc);
            await _localStorageService.SetItem(_userKey, Account);
        }
        public async Task<IEnumerable<AccountModel>> GetAccounts()
        {
            var response = await _httpService.Get<List<AccountModel>>("/Account");
            return response;
        }

        public async Task InsertAccount(AccountModel account)
        {
            await _httpService.Post<AccountModel>("/Account", account);
        }
        public async Task<IEnumerable<AccountModel>> SearchAccount(string username)
        {
            return await _httpService.Get<List<AccountModel>>("/Account/?username=" + username); // client route-passing receive
        }

        public async Task EditAccount(AccountModel account)
        {
            await _httpService.Put<AccountModel>("/Account", account);
        }

        public async Task DeleteAccount(AccountModel account)
        {
            await _httpService.Delete("/Account/?username=" + account.account_username);
        }


    }
}
