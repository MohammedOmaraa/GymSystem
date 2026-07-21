
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.AnalyticsViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;

namespace GymSystem.BLL.Services.Classes
{
    public class AnalyticsServices : IAnalyticsServices
    {
        private readonly IUnitOfWork unitOfWork;

        public AnalyticsServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default)
        {
            var Sessions = await unitOfWork.GetRepository<Session>().GetAllAsync(false, ct);

            var TotalMembers = await unitOfWork.GetRepository<Member>().CountAsync();

            var TotalTrainers = await unitOfWork.GetRepository<Trainer>().CountAsync();

            var ActiveMembers = await unitOfWork.GetRepository<Membership>().CountAsync(m => m.EndDate > DateTime.Now, ct);

            return new AnalyticsViewModel
            {
                TotalMembers = TotalMembers,
                ActiveMembers = ActiveMembers,
                TotalTrainers = TotalTrainers,
                UpcomingSessions = Sessions.Count(s => s.StartDate > DateTime.Now),
                OngoingSessions = Sessions.Count(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now),
                CompletedSessions = Sessions.Count(s => s.EndDate < DateTime.Now)
            };

        }
    }
}
