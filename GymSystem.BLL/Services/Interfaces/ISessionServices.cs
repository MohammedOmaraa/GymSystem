using GymSystem.BLL.ViewModels.SessionsViewModels;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ISessionServices
    {
        public Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct);
    }
}
