using AutoMapper;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionsViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;

namespace GymSystem.BLL.Services.Classes
{
    public class SessionServices : ISessionServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct)
        {
            var Sessions = await unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);
            if (!Sessions.Any()) return null;
            Sessions = Sessions.OrderByDescending(s => s.StartDate);
            var MappedSessions = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);
            

            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);
            }
            return MappedSessions;
        }
    }
}
