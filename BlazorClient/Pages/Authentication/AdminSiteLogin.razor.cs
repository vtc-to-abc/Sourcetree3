using Microsoft.AspNetCore.Components;
using BlazorClient.Models;
using BlazorClient.Services;
using BlazorClient.Helpers;

namespace BlazorClient.Pages.Authentication
{
    public partial class AdminSiteLogin:ComponentBase
    {
        [Inject] private IAccountService AccountService { get; set; }
        //[Inject] private IAlertService AlertService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        private AccountModel Acc = new();
        private bool loading;
        public async Task Login()
        {
            loading = true;
            try
            {
                await AccountService.Login(Acc);
                // try to login the requesting account then return the redirect url
                var returnUrl = NavigationManager.QueryString("returnUrl") ?? "";
                NavigationManager.NavigateTo(returnUrl);
            }
            catch(Exception ex)
            {
                loading = false;
                StateHasChanged();

            }
        }

    }
}
