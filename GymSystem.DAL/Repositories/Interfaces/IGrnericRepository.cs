using GymSystem.DAL.Entities;

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
    }
}
