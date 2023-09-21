using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NomenclaturesController : ControllerBase, IDisposable
{
    private UnitOfWork _unitOfWork = new();
    private bool disposedValue;

    [HttpGet]
    public async Task<ActionResult<DbSet<Nomenclature>>> GetAll()
    {
        return Ok(_unitOfWork.NomenclatureRepository.GetAll());
    }

    [HttpPost]
    public async Task<ActionResult<DbSet<Nomenclature>>> Add(Nomenclature nomenclature)
    {
        _unitOfWork.NomenclatureRepository.Add(nomenclature);
        _unitOfWork.Save();

        return Ok(_unitOfWork.NomenclatureRepository.GetAll());
    }

    [HttpPut]
    public async Task<ActionResult<DbSet<Nomenclature>>> Update(Nomenclature nomenclature)
    {
        _unitOfWork.NomenclatureRepository.Update(nomenclature);
        _unitOfWork.Save();

        return Ok(_unitOfWork.NomenclatureRepository.GetAll());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DbSet<Nomenclature>>> Delete(int id)
    {
        var user = await _unitOfWork.NomenclatureRepository.GetAll().FindAsync(id);

        if (user == null)
            return NotFound(new { message = "User not found" });

        _unitOfWork.NomenclatureRepository.Delete(user);
        _unitOfWork.Save();

        return Ok(_unitOfWork.NomenclatureRepository.GetAll());
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