using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/login");
builder.Services.AddAuthorization();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/login", async (HttpContext context) =>
{
    var user1 = context.User.Identity;
    if (user1 is not null && user1.IsAuthenticated)
    {
        context.Response.Redirect("/");
    }
    else
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        string loginForm = @"<!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8' />
                <title>Authentication</title>
            </head>
            <body>
                <h2>Login Form</h2>
                <form method='post'>
                    <p>
                        <label>Login</label><br />
                        <input name='login' />
                    </p>
                    <p>
                        <label>Password</label><br />
                        <input type='password' name='password' />
                    </p>
                    <input type='submit' value='Login' />
                </form>
            </body>
            </html>";
        await context.Response.WriteAsync(loginForm);
    }
});

app.MapPost("/login", async (string? returnUrl, HttpContext context) =>
{
    var form = context.Request.Form;

    if (!form.ContainsKey("login") || !form.ContainsKey("password"))
        return Results.BadRequest("Имя и/или пароль не  установлены");

    string? name = form["login"];
    string? password = form["password"];

    using (var db = new ERPContext())
    {
        var user = db.Users.FirstOrDefault(x => x.Login == name && x.Password == password);

        if (user is null) return Results.Unauthorized();

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login) };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }

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

    await context.Response.WriteAsync("<html>Панель админа</html>");
});

app.Run();

// using (ERPContext db = new ERPContext())
// {
//     var users = db.Users.ToList();
//     Console.WriteLine("Users list:");
//     foreach (User u in users)
//     {
//         Console.WriteLine($"{u.Id}.{u.Name} - {u.Password}");
//     }
// }


// using (ERPContext db = new ERPContext())
// {
//     User user = new User { Name = "admin", Password = "admin" };

//     db.Users.AddRange(user);
//     db.SaveChanges();
// }

// record class User1(string Name, string Password);


