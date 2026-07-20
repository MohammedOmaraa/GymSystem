using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace GymSystem.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext dbContext;

        public SessionRepository(GymDbContext dbContext): base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct)
        {
            var Sessions = dbContext.Sessions.AsNoTracking()
                                             .Include(s => s.Trainer)
                                             .Include(s => s.Category);

            return await Sessions.ToListAsync(ct);
        }


        public Task<int> GetCountOfBookedSlotAsync(int sessionId, CancellationToken ct)
        {
            return dbContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId);
        }

        public async Task<Session> GetSessionByIdWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct)
        {
            var Session = dbContext.Sessions.Include(s => s.Trainer)
                                            .Include(s => s.Category)
                                            .FirstOrDefaultAsync(s => s.Id == sessionId, ct);

            return await Session;
        }
    }
}
