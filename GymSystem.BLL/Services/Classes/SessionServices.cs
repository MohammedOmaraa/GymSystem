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

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct)
        {
            var Categories = await unitOfWork.GetRepository<Category>().GetAllAsync(false, ct);

            return mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(Categories);
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct)
        {
            var Trainers = await unitOfWork.GetRepository<Trainer>().GetAllAsync(false, ct);

            return mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(Trainers);
        }

        async Task<bool> ISessionServices.CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct)
        {
            if (model.StartDate >= model.EndDate)
                return false;

            if (model.StartDate <= DateTime.Now)
                return false;

            var TrainerRepo = unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetByIdAsync(model.TrainerId, ct);

            if (Trainer is null)
                return false;

            var CategoryRepo = unitOfWork.GetRepository<Category>();
            var Category = await CategoryRepo.GetByIdAsync(model.CategoryId, ct);

            if (Category is null)
                return false;

            var Session = mapper.Map<CreateSessionViewModel, Session>(model);

            var SessionRepo = unitOfWork.GetRepository<Session>();

            SessionRepo.Add(Session);

            var Result = await unitOfWork.CompeleteAsync();

            return Result > 0;
        }
    }
}
