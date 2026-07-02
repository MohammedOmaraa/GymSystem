using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;

namespace GymSystem.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext dbContext;
        private readonly Dictionary<string, object> repositories = [];

        public UnitOfWork(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> CompeleteAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public IGrnericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName = typeof(TEntity).Name;

            if (repositories.TryGetValue(TypeName, out object OldRepository))

                return (IGrnericRepository<TEntity>)OldRepository;

            var newRepository = new GenericRepository<TEntity>(dbContext);
            repositories[TypeName] = newRepository;
            return newRepository;
        }
    }
}
