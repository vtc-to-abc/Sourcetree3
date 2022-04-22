using BlazorServer.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<IAuthorData, AuthorData>();
builder.Services.AddTransient<IBookData, BookData>();
builder.Services.AddTransient<IAuthorBookData, AuthorBookData>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson();
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
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.UseStaticFiles();

app.UseRouting();


app.Run();
