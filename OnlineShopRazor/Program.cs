using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("OnlineDataBaseContext") ?? throw new InvalidOperationException("Connection string 'OnlineDataBaseContext' not found.");

builder.Services.AddDbContext<OnlineDataBaseContext>(options => options.UseSqlServer(connectionString));

//-------------------------
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
//-------------------------

// Add services to the container.
builder.Services.AddRazorPages();
//-------------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Login";
            options.LogoutPath = "/logout";
        });

//-------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();


app.MapGet("/all-routes", (EndpointDataSource endpointDataSource) =>
{
    var endpoints = endpointDataSource.Endpoints
        .OfType<RouteEndpoint>()
        .Select(e => new
        {
            Route = e.RoutePattern.RawText,
            DisplayName = e.DisplayName
        });

    return Results.Ok(endpoints);
});


//-------------------
app.UseSession();
//-------------------
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
