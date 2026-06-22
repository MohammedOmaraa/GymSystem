using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DAL.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext dbContext;
        public PlanRepository(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(Plan plan)
        {
            dbContext.Plans.Add(plan);
        }

        public async Task<int> CompleteAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var plan = dbContext.Plans.FirstOrDefault(p => p.Id == id);
            if (plan != null)
                dbContext.Plans.Remove(plan);
        }

        public async Task<IEnumerable<Plan>> GetAllPlansAsync()
        {
            //return Task.FromResult<IEnumerable<Plan>>(_dbContext.Plans);
            return await this.dbContext.Plans.ToListAsync();
        }

        public async Task<Plan?> GetPlanByIdAsync(int id)
        {
            return await dbContext.Plans.FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Update(Plan plan)
        {
            dbContext.Plans.Update(plan);
        }
    }
}
