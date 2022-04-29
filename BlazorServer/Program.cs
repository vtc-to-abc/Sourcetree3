using BlazorServer.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity.UI;
using BlazorServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlazorServer.Data.Services;
using BlazorServer.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
//using static BlazorServer.Data.IRolePermissionData;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddScoped<IAuthorData, AuthorData>();
builder.Services.AddScoped<IBookData, BookData>();
builder.Services.AddScoped<IAuthorBookData, AuthorBookData>();
builder.Services.AddScoped<IAccountData, AccountData>();
builder.Services.AddScoped<IPermissionData, PermissionData>();
builder.Services.AddScoped<IRoleData, RoleData>();
builder.Services.AddScoped<IRolePermissionData, RolePermissionData>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.Configure<ApiBehaviorOptions>(options =>  // to avoid return badrequest instantly when validate fail
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(jwtOptions =>
    {
        var key = builder.Configuration.GetValue<string>("JwtConfig:Key");
        var keyBytes = Encoding.ASCII.GetBytes(key);
        jwtOptions.SaveToken = true;
        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateAudience = false,



        };

      
    });



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdmin", policy =>
        policy.RequireClaim(ClaimTypes.Role, 999.ToString()));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
/// <summary>
/// The response-header fields allow the server to pass additional
/// information about the response which cannot be placed in the Status-
/// Line. These header fields give information about the server and about
/// further access to the resource identified by the Request-URI.
/// </summary>
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Type", "application/json; charset=utf-8"); // header, determine what content type will the respone of a request will return
    await next();
});


app.UseCors(builder => builder.WithOrigins("https://localhost:7025")
                                .AllowAnyMethod()
                                .AllowAnyHeader());/* accept all header from https://localhost:7025*/


app.UseMiddleware<JwtMiddleware>();



app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
