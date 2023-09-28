using Microsoft.AspNetCore.Mvc;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAll();
    Task Create(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(int? id);
}