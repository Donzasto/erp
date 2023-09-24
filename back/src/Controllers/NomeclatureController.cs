using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class NomenclatureController : ControllerBase
{
    private readonly ERPContext _eRPContext;

    public NomenclatureController(ERPContext eRPContext)
    {
        _eRPContext = eRPContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<Nomenclature>>> GetAll()
    {
        return await _eRPContext.Nomenclatures.AsNoTracking().ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<DbSet<Nomenclature>>> Create(Nomenclature nomenclature)
    {
        try
        {
            _eRPContext.Add(nomenclature);
            await _eRPContext.SaveChangesAsync();

            return Ok();
        }
        catch (DbUpdateException)
        {
            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<ActionResult<DbSet<Nomenclature>>> Update(Nomenclature entity)
    {
        _eRPContext.Entry(entity).State = EntityState.Modified;

        try
        {
            await _eRPContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult<DbSet<Nomenclature>>> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nomenclature = await _eRPContext.Nomenclatures.
            AsNoTracking().
            FirstOrDefaultAsync(n => n.Id == id);

        if (nomenclature == null)
        {
            return NotFound();
        }

        _eRPContext.Nomenclatures.Remove(nomenclature);
        await _eRPContext.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("rootNodeId")]
    public async Task<ActionResult<DbSet<NomenclatureRelations>>> GetProductTreeNodesIds(int rootNodeId)
    {
        var context = new ERPContext();

        DbSet<NomenclatureRelations> DbSetNomenclatureRelations = context.Set<NomenclatureRelations>();

        var nodesIds = await DbSetNomenclatureRelations.FromSql(
            $@"WITH RECURSIVE entity AS (
            SELECT id, parrent_id, child_id 
            FROM nomenclature_relations 
            WHERE parrent_id={rootNodeId}
            UNION SELECT nr.id, nr.parrent_id, nr.child_id
            SELECT FROM nomenclature_relations AS nr
            INNER JOIN entity e on e.child_id = nr.parrent_id
            ) SELECT * FROM entity").AsNoTrackingWithIdentityResolution().ToListAsync();

        return Ok(nodesIds);
    }
}