// unset

using System.Linq.Expressions;
using CraftersCloud.Core.Entities;

namespace CraftersCloud.Core.Data
{
    public interface IEntityRepository<T> where T : Entity
    {
        void Add(T entity);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

        IQueryable<T> QueryAll();

        IQueryable<T> QueryAllSkipCache();

        IQueryable<T> QueryAllIncluding(params Expression<Func<T, object>>[] paths);

        IQueryable<T> QueryAllSkipCacheIncluding(params Expression<Func<T, object>>[] paths);
    }
}
