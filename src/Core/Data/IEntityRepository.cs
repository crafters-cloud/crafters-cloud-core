using CraftersCloud.Core.Entities;

namespace CraftersCloud.Core.Data;

public interface IEntityRepository<T> where T : Entity
{
    IQueryable<T> QueryAll();
    void Add(T entity);

    void Delete(T entity);

    void DeleteRange(IEnumerable<T> entities);
}