using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

internal class GenericRepository1<TEntity> where TEntity : class
{
    private ERPContext _eRPContext = new();
    private DbSet<TEntity> _dbSet;

    internal GenericRepository1()
    {
        // _eRPContext = eRPContext;
        _dbSet = _eRPContext.Set<TEntity>();
    }

    internal async Task<List<TEntity>> GetAll()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    internal async Task<List<TEntity>> Add(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _eRPContext.SaveChangesAsync();

        return await GetAll();
    }

    internal async Task<List<TEntity>> Update(TEntity entity)
    {
        // _dbSet.Attach(entity);
        // _eRPContext.Entry(entity).State = EntityState.Modified;
        // _dbSet.Update(entity);

        await _eRPContext.SaveChangesAsync();

        return await GetAll();
    }

    internal async Task<List<TEntity>> Delete(TEntity entity)
    {
        //  var entity = await _unitOfWork.NomenclatureRepository.GetAll().FindAsync(id);

        // if (entity == null)
        //     return NotFound(new { message = "Nomenclature not found" });

        _dbSet.Remove(entity);

        await _eRPContext.SaveChangesAsync();

        return await GetAll();
    }
}