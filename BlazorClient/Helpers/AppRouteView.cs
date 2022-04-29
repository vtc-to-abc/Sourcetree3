using Microsoft.AspNetCore.Components;
using BlazorClient.Services;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System;

namespace BlazorClient.Helpers
{
    //use for redirect when login
    public class AppRouteView: RouteView
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAccountService AccountService { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            // check request acc authorize.
            // if authorize fail or the requesting acc is not correct then redirect back to login page
            var account = AccountService.Account;
            var authorize = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) != null;

            if (authorize && account == null)
            {
                var returnUrl = WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery);
                NavigationManager.NavigateTo($"/login?returnUrl={returnUrl}");
            }
            // else redirect to admin page(the default in program.cs)
            else
                base.Render(builder);
        }
    }
}
