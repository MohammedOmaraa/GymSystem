
using GymSystem.DAL.Entities;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface ISessionRepository:IGrnericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct);
        Task<Session> GetSessionByIdWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct);
        Task<int> GetCountOfBookedSlotAsync(int sessionId, CancellationToken ct);
    }
}
