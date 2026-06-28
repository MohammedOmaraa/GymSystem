using GymSystem.DAL.Entities;
using System.Linq.Expressions;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IGrnericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool isTracked, CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        void Add(TEntity item);
        void Update(TEntity item);
        void Delete(int id);
        Task<int> CompleteAsync();
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked, CancellationToken ct = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

    }
}
