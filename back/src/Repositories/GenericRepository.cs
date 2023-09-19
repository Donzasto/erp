using Microsoft.EntityFrameworkCore;

internal class GenericRepository<TEntity> where TEntity : class
{
    private ERPContext _eRPContext;
    private DbSet<TEntity> _dbSet;

    internal GenericRepository(ERPContext eRPContext)
    {
        _eRPContext = eRPContext;
        _dbSet = eRPContext.Set<TEntity>();
    }

    internal DbSet<TEntity> GetAll()
    {
        return _dbSet;
    }

    internal void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    internal void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _eRPContext.Entry(entity).State = EntityState.Modified;
    }

    internal void Delete(TEntity entity)
    {
        if (_eRPContext.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);
    }
}