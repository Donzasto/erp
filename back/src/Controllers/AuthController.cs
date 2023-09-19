using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class AuthController : ControllerBase
{
    private ERPContext _eRPContext;

    public AuthController(ERPContext eRPContext)
    {
        _eRPContext = eRPContext;
    }

    [Route("api/login")]
    [HttpPost]
    public async Task<ActionResult<User>> Login()
    {
        var form = Request.Form;

        if (!form.ContainsKey("login") || !form.ContainsKey("password"))
            return BadRequest("Имя и/или пароль не  установлены");

        string? login = form["login"];
        string? password = form["password"];

        var dbSet = _eRPContext.Set<User>();
        var user = dbSet.FirstOrDefault(x => x.Login == login && x.Password == password);

        if (user is null) return Unauthorized();

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "") };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return Redirect("/");
    }

    [Route("api/login")]
    [HttpGet]
    public async Task<ActionResult<User>> GetIsAuthenticated()
    {
        var user = HttpContext.User.Identity;

        if (user is not null && user.IsAuthenticated)
        {
            return Ok();
        }
        else
        {
            return Unauthorized();
        }
    }

    [Route("api/logout")]
    [HttpGet]
    public async Task<ActionResult<User>> GetLogout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Redirect("/");
    }
}