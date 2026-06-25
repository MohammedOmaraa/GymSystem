using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGrnericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext dbContext;
        public GenericRepository(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(TEntity item)
        {
            dbContext.Set<TEntity>().Add(item);
        }

        public async Task<int> CompleteAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var item = dbContext.Set<TEntity>().FirstOrDefault(p => p.Id == id);
            if (item != null)
                dbContext.Set<TEntity>().Remove(item);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool isTracked, CancellationToken ct = default)
        {
            var items = isTracked ? dbContext.Set<TEntity>() : dbContext.Set<TEntity>().AsNoTracking();
            return await items.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var item = await dbContext.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == id, ct);
            return item;
        }

        public void Update(TEntity item)
        {
            dbContext.Set<TEntity>().Update(item);
        }
    }
}
