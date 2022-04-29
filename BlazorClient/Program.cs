using BlazorClient;
using BlazorClient.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Headers;
using MudBlazor.Services;
using Blazored.Modal;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddBlazoredModal();

builder.Services.AddApiAuthorization();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

/// <summary>
///    The request-header fields allow the client to pass additional
///    information about the request, and about the client itself, to the
///    server. These fields act as request modifiers, with semantics
///    equivalent to the parameters on a programming language method
///    invocation.
/// </summary>
/// set up client side
builder.Services.AddScoped(sp => {
    var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7051") };
    httpClient.DefaultRequestHeaders// request header
      .Accept // what server reponse type will this side's request accept.
      .Add(new MediaTypeWithQualityHeaderValue("application/json")); // this side accept "application/json" response type
        
    return httpClient;
    });
// must notice about dependency lifespan
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorBookService, AuthorBookService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IHttpService, HttpService>();



var host = builder.Build();

var accountService = host.Services.GetRequiredService<IAccountService>();
await accountService.Initialize();

await host.RunAsync();
