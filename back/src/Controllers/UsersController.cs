using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "AdminAccess")]
public class UsersController : ControllerBase, IDisposable
{
    // private readonly ERPContext _eRPContext;
    private UnitOfWork _unitOfWork = new();
    private bool disposedValue;

    // public UsersController(ERPContext eRPContext)
    // {
    // _eRPContext = eRPContext;
    // }

    [HttpGet]
    public async Task<ActionResult<DbSet<User>>> GetAllUsers()
    {
        return Ok(_unitOfWork.UsersRepository.GetAll());
        // return Ok(_eRPContext.Users);
    }

    [HttpPost]
    public async Task<ActionResult<DbSet<User>>> AddUser(User userRequest)
    {
        // var user = new User() { Login = userRequest.Login, Password = userRequest.Password };

        // await _eRPContext.Users.AddAsync(user);
        // await _eRPContext.SaveChangesAsync();

        _unitOfWork.UsersRepository.Add(userRequest);
        _unitOfWork.Save();

        return Ok(_unitOfWork.UsersRepository.GetAll());
    }

    [HttpPut]
    public async Task<ActionResult<DbSet<User>>> UpdateUser(User userRequest)
    {
        // var users = _unitOfWork.UsersRepository.GetAll().AsNoTracking();
        // var user = await users.FirstOrDefaultAsync(u => u.Id == userRequest.Id);

        // if (user == null)
        //     return NotFound(new { message = "User not found" });

        // user.Login = userRequest.Login;
        // // user.Password = userRequest.Password;


        _unitOfWork.UsersRepository.Update(userRequest);
        _unitOfWork.Save();
        // await _eRPContext.SaveChangesAsync();

        return Ok(_unitOfWork.UsersRepository.GetAll());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DbSet<User>>> DeleteUser(int id)
    {
        var user = await _unitOfWork.UsersRepository.GetAll().FindAsync(id);

        if (user == null)
            return NotFound(new { message = "User not found" });

        // _eRPContext.Users.Remove(user);
        // await _eRPContext.SaveChangesAsync();

        _unitOfWork.UsersRepository.Delete(user);
        _unitOfWork.Save();

        return Ok(_unitOfWork.UsersRepository.GetAll());
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }

            disposedValue = true;
        }
    }

    void IDisposable.Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}