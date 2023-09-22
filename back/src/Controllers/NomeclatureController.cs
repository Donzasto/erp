using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
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
        var entity = await _unitOfWork.NomenclatureRepository.GetAll().FindAsync(id);

        if (entity == null)
            return NotFound(new { message = "Nomenclature not found" });

        _unitOfWork.NomenclatureRepository.Delete(entity);
        _unitOfWork.Save();

        return Ok(_unitOfWork.NomenclatureRepository.GetAll());
    }

    // public async Task<IActionResult> GetTree()
    // {

    //     return Ok();
    // }

    /*
    WITH RECURSIVE nomenclaturea AS (
    SELECT id_parrent, id_child 
    FROM nomenclature_relations
    WHERE id=1
    UNION SELECT nr.id_parrent, nr.id_child
      SELECT FROM nomenclature_relations AS nr
      INNER JOIN nomenclaturea n on n.id_child = nr.id_parrent
    ) SELECT 	* FROM nomenclaturea*/
    [HttpGet("id")]
    public async Task<IActionResult> GetNomenclatureRelations(int id)
    {
        var context = new ERPContext();
        DbSet<NomenclatureRelations> nr = context.Set<NomenclatureRelations>();

        var aa = nr.FromSql($@"WITH RECURSIVE nomenclaturea AS (
        SELECT ss.id, ss.id_parrent, ss.id_child 
        FROM nomenclature_relations ss
        WHERE id_parrent = {id}
        UNION SELECT nr.id, nr.id_parrent, nr.id_child
          SELECT FROM nomenclature_relations AS nr
          INNER JOIN nomenclaturea n on n.id_child = nr.id_parrent
        ) SELECT  * FROM nomenclaturea").ToListAsync();

        return Ok(aa);
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