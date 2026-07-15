using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.SessionsViewModels;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ISessionServices
    {
        public Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);
        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct);
        Task<SessionViewModel?> GetSessionByIdAsync(int sessionId, CancellationToken ct);
        Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct);
        Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct= default);
        Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct = default);

    }
}
