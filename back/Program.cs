using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/login");
builder.Services.AddAuthorization();
builder.Services.AddDbContext<ERPContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Database=erp;Username=postgres;Password=mysecretpassword"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.AddSecurityDefinition("Cookies", new Microsoft.OpenApi.Models.OpenApiSecurityScheme { }));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    return Results.Redirect("/");
});

app.MapPost("/api/login", async (string? returnUrl, HttpContext context, ERPContext db) =>
{
    var form = context.Request.Form;

    if (!form.ContainsKey("login") || !form.ContainsKey("password"))
        return Results.BadRequest("Имя и/или пароль не  установлены");

    string? login = form["login"];
    string? password = form["password"];

    var user = db.Users.FirstOrDefault(x => x.Login == login && x.Password == password);

    if (user is null) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login) };

    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

    return Results.Redirect(returnUrl ?? "/");
});

app.MapGet("/api/users", [Authorize] async (HttpContext context, ERPContext eRPContext) =>
{
    await context.Response.WriteAsJsonAsync(eRPContext.Users);
});

app.MapGet("/api/login", (HttpContext context) =>
{
    var user = context.User.Identity;

    if (user is not null && user.IsAuthenticated)
    {
        return Results.Ok();
    }
    else
    {
        return Results.Unauthorized();
    }
});

app.MapPost("/api/users", async (HttpContext context, ERPContext eRPContext) =>
{
    var form = context.Request.Form;
    // if (!form.ContainsKey("login") || !form.ContainsKey("password"))
    //     return Results.BadRequest("Имя и/или пароль не  установлены");

    string? login = form["login"];
    string? password = form["password"];

    var user = new User() { Login = login, Password = password };

    await eRPContext.Users.AddAsync(user);
    await eRPContext.SaveChangesAsync();
    return user;
});

app.MapDelete("/api/users/{id:int}", async (int id, ERPContext eRPContext) =>
{
    User? user = await eRPContext.Users.FirstOrDefaultAsync(u => u.Id == id);

    if (user == null) return Results.NotFound(new { message = "User not found" });

    eRPContext.Users.Remove(user);
    await eRPContext.SaveChangesAsync();
    return Results.Json(user);
});

app.Run();