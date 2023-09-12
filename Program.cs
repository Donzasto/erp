using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/login");
builder.Services.AddAuthorization();
builder.Services.AddDbContext<ERPContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Database=erp;Username=postgres;Password=mysecretpassword"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();


app.MapGet("/login", async (HttpContext context) =>
{
    var user = context.User.Identity;

    if (user is not null && user.IsAuthenticated)
    {
        context.Response.Redirect("/");
    }
    else
    {
        context.Response.ContentType = "text/html; charset=utf-8";

        await context.Response.SendFileAsync("wwwroot/login.html");
    }
});

app.MapPost("/login", async (string? returnUrl, HttpContext context, ERPContext db) =>
{
    var form = context.Request.Form;

    if (!form.ContainsKey("login") || !form.ContainsKey("password"))
        return Results.BadRequest("Имя и/или пароль не  установлены");

    string? name = form["login"];
    string? password = form["password"];

    var user = db.Users.FirstOrDefault(x => x.Login == name && x.Password == password);

    if (user is null) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login) };

    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

    return Results.Redirect(returnUrl ?? "/");
});

app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    return Results.Redirect("/");
});

app.MapGet("/product-composition", () => "product-composition");

app.MapGet("/manage-product-composition", [Authorize] () => "manage-product-composition");

app.MapGet("/admin-panel", [Authorize] async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";

    await context.Response.SendFileAsync("wwwroot/admin-panel.html");
});

app.Run();