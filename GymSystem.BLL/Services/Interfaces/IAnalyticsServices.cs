
using GymSystem.BLL.ViewModels.AnalyticsViewModels;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IAnalyticsServices
    {
        Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default);
    }
}
