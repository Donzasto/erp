using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Implement CRUD functionality for Nomenclature and NomenclatureRelation models.
/// All methods may returns one of two status codes:
/// code="200" > Operation successfully
/// code="400" > Something is going wrong. See ExceptionHandlerMiddleware.cs
/// Additional return status codes are described in the method comments.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public class NomenclatureController : ControllerBase
{
    private readonly ERPContext _eRPContext;

    /// <summary>
    /// The only constructor.
    /// </summary>
    /// <param name="eRPContext"></param>
    public NomenclatureController(ERPContext eRPContext)
    {
        _eRPContext = eRPContext;
    }

    /// <summary>
    /// Get list of all nomenclature.
    /// </summary>
    /// <returns>List of all nomenclature</returns>
    [HttpGet]
    public async Task<ActionResult<List<Nomenclature>>> GetAll()
    {
        return Ok(await _eRPContext.Nomenclatures.AsNoTracking().ToListAsync());
    }

    /// <summary>
    /// Create a nomeclature.
    /// </summary>
    /// <param name="nomenclature"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> Create(Nomenclature nomenclature)
    {
        _eRPContext.Add(nomenclature);
        await _eRPContext.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Update a specific nomenclature.
    /// </summary>
    /// <param name="nomenclature"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult> Update(Nomenclature nomenclature)
    {
        _eRPContext.Entry(nomenclature).State = EntityState.Modified;

        await _eRPContext.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Delete a specific nomenclature.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="404">id or nomenclature not found</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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