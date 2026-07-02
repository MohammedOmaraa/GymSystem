
using GymSystem.DAL.Entities;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public IGrnericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();

        public Task<int> CompeleteAsync();

    }
}
