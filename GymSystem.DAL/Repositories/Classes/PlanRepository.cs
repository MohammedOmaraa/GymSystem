using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DAL.Repositories.Classes
{
    public class PlanRepository : GenericRepository<Plan>, IPlanRepository
    {
        private readonly GymDbContext dbContext;
        public PlanRepository(GymDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
