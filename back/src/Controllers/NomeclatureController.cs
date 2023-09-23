using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class NomenclatureController : ControllerBase
{
    private GenericRepository1<Nomenclature> _genericRepository;

    public NomenclatureController()
    {
        _genericRepository = new GenericRepository1<Nomenclature>();
    }

    [HttpGet]
    public async Task<ActionResult<DbSet<Nomenclature>>> GetAll()
    {
        return Ok(await _genericRepository.GetAll());
    }

    [HttpPost]
    public async Task<ActionResult<DbSet<Nomenclature>>> Add(Nomenclature nomenclature)
    {
        await _genericRepository.Add(nomenclature);

        return Ok(await _genericRepository.GetAll());
    }

    [HttpPut]
    public async Task<ActionResult<DbSet<Nomenclature>>> Update(Nomenclature nomenclature)
    {
        await _genericRepository.Update(nomenclature);

        return Ok(await _genericRepository.GetAll());
    }

    [HttpDelete]
    public async Task<ActionResult<DbSet<Nomenclature>>> Delete(Nomenclature entity)
    {
        await _genericRepository.Delete(entity);

        return Ok(await _genericRepository.GetAll());
    }

    [HttpGet("id")]
    public async Task<ActionResult<DbSet<NomenclatureRelations>>> GetNomenclatureRelations(int id)
    {
        var context = new ERPContext();
        DbSet<NomenclatureRelations> nr = context.Set<NomenclatureRelations>();

        var aa = await nr.FromSql($@"WITH RECURSIVE nomenclaturea AS (
            SELECT id, parrent_id, child_id 
            FROM nomenclature_relations ss
            WHERE ss.parrent_id={id}
            UNION SELECT nr.id, nr.parrent_id, nr.child_id
            SELECT FROM nomenclature_relations AS nr
            INNER JOIN nomenclaturea n on n.child_id = nr.parrent_id
            ) SELECT * FROM nomenclaturea").AsNoTrackingWithIdentityResolution().ToListAsync();

        return Ok(aa);
    }
}