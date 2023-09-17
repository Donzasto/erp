using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ERPContext _eRPContext;
    public UsersController(ERPContext eRPContext)
    {
        _eRPContext = eRPContext;
    }

    [HttpGet]
    public async Task<ActionResult<DbSet<User>>> GetAllUsers()
    {
        return Ok(_eRPContext.Users);
    }

    [HttpPost]
    public async Task<ActionResult<DbSet<User>>> AddUser(User userRequest)
    {
        if (string.IsNullOrWhiteSpace(userRequest.Login) || string.IsNullOrWhiteSpace(userRequest.Password))
            return BadRequest("Имя и/или пароль не  установлены");

        var user = new User() { Login = userRequest.Login, Password = userRequest.Password };

        await _eRPContext.Users.AddAsync(user);
        await _eRPContext.SaveChangesAsync();

        return Ok(_eRPContext.Users);
    }

    [HttpPut]
    public async Task<ActionResult<DbSet<User>>> UpdateUser(User userRequest)
    {
        var user = await _eRPContext.Users.FindAsync(userRequest.Id);

        if (user == null)
            return NotFound(new { message = "User not found" });

        user.Login = userRequest.Login;
        user.Password = userRequest.Password;

        await _eRPContext.SaveChangesAsync();

        return Ok(_eRPContext.Users);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DbSet<User>>> DeleteUser(int id)
    {
        var user = await _eRPContext.Users.FindAsync(id);

        if (user == null)
            return NotFound(new { message = "User not found" });

        _eRPContext.Users.Remove(user);
        await _eRPContext.SaveChangesAsync();

        return Ok(_eRPContext.Users);
    }
}