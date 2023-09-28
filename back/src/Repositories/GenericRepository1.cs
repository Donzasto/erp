using Microsoft.EntityFrameworkCore;

internal class GenericRepository1<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private ERPContext _eRPContext = new();
    private DbSet<TEntity> _dbSet;

    public GenericRepository1()
    {
        // _eRPContext = eRPContext;
        _dbSet = _eRPContext.Set<TEntity>();
    }

    public async Task Create(TEntity entity)
    {
        _dbSet.Add(entity);
        await _eRPContext.SaveChangesAsync();
    }

    public async Task Delete(int? id)
    {
        if (id == null)
        {
            return;
        }

        var entity = await _dbSet.FindAsync(id);

        if (entity == null)
        {
            return;
        }

        _dbSet.Remove(entity);

        await _eRPContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task Update(TEntity entity)
    {
        _dbSet.Entry(entity).State = EntityState.Modified;
        await _eRPContext.SaveChangesAsync();
    }
}