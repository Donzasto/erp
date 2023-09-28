using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "AdminAccess")]
public class UsersController : ControllerBase
{
    private readonly ERPContext _eRPContext;

    public UsersController(ERPContext eRPContext)
    {
        _eRPContext = eRPContext;
    }

    /// <summary>
    /// Get list of all users.
    /// </summary>
    /// <returns>List of all users</returns>
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        return Ok(await _eRPContext.Users.AsNoTracking().ToListAsync());
    }

    /// <summary>
    /// Create a nomeclature.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> Create(User entity)
    {
        _eRPContext.Add(entity);
        await _eRPContext.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Update a specific user.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult> Update(User entity)
    {
        _eRPContext.Entry(entity).State = EntityState.Modified;

        await _eRPContext.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Delete a specific user.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="404">id or user not found</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var entity = await _eRPContext.Users.
            AsNoTracking().
            FirstOrDefaultAsync(n => n.Id == id);

        if (entity == null)
        {
            return NotFound();
        }

        _eRPContext.Users.Remove(entity);

        await _eRPContext.SaveChangesAsync();

        return Ok();
    }
}