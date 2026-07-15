using AutoMapper;
using GymSystem.BLL.Common;
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

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct)
        {
            var validation = ValidateDates(model.StartDate, model.EndDate);

            if (!validation.IsSuccess)
                return validation;

            var TrainerRepo = unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetByIdAsync(model.TrainerId, ct);

            if (Trainer is null)
                return Result.NotFound("Trainer not found.");

            var CategoryRepo = unitOfWork.GetRepository<Category>();
            var Category = await CategoryRepo.GetByIdAsync(model.CategoryId, ct);

            if (Category is null)
                return Result.NotFound("Category not found.");

            var Session = mapper.Map<CreateSessionViewModel, Session>(model);

            var SessionRepo = unitOfWork.GetRepository<Session>();

            SessionRepo.Add(Session);

            var affectedRows = await unitOfWork.CompeleteAsync();

            return affectedRows > 0 ? Result.Success() : Result.Failure("Failed to create session.");
        }

        public async Task<SessionViewModel?> GetSessionByIdAsync(int sessionId, CancellationToken ct)
        {
            var Session = await unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategoryAsync(sessionId, ct);
            if (Session == null)
                return null;

            var mappedSession = mapper.Map<Session, SessionViewModel>(Session);

            mappedSession.AvailableSlots = mappedSession.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(mappedSession.Id, ct);

            return mappedSession;
        }

        private Result ValidateDates(DateTime start, DateTime end)
        {
            if (start >= end)
                return Result.Validation("End date must be after start date.");

            if (start <= DateTime.UtcNow)
                return Result.Validation("Start date must be in the future.");

            return Result.Success();
        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct)
        {
            var Session = await unitOfWork.GetRepository<Session>().GetByIdAsync(sessionId, ct);
            if (Session is null)
                return Result<UpdateSessionViewModel>.NotFound("Session not found.");

            var validation = await CanEditSessionAsync(Session, ct);

            if (!validation.IsSuccess)
                return Result<UpdateSessionViewModel>.Failure(validation.Message!, validation.Kind);

            var model = mapper.Map<Session, UpdateSessionViewModel>(Session);
            return Result<UpdateSessionViewModel>.Success(model);
        }

        public async Task<Result> CanEditSessionAsync(Session session, CancellationToken ct)
        {
            if (session.StartDate <= DateTime.UtcNow)
                return Result.Failure("Session has already started.");

            var booked = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);
            if (booked > 0)
                return Result.Failure(
                    "Session already has bookings.");

            return Result.Success();
        }

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var SessionRepo = unitOfWork.GetRepository<Session>();
            var Session = await SessionRepo.GetByIdAsync(id, ct);

            if (Session is null)
                return Result.NotFound("Session not found.");

            if (Session.StartDate <= DateTime.UtcNow)
                return Result.Failure("Can't edit a session that has already started.");

            var BookedCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(Session.Id, ct);

            if (BookedCount > 0)
                return Result.Failure("Can't edit a session that has booked slots.");

            var validation = ValidateDates(model.StartDate, model.EndDate);

            if (!validation.IsSuccess)
                return validation;

            var TrainerRepo = unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetByIdAsync(model.TrainerId, ct);

            if (Trainer is null)
                return Result.NotFound("Trainer not found.");

            Session.UpdatedAt = DateTime.UtcNow;

            mapper.Map(model, Session);
            SessionRepo.Update(Session);

            var AffectedRows = await unitOfWork.CompeleteAsync();

            return AffectedRows > 0 ? Result.Success() : Result.Failure("Failed to update session.");
        }
    }
}
