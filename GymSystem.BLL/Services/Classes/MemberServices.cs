using GymSystem.BLL.Services.Interfaces;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystemG03.BLL.ViewModels.MembersViewModels;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IGrnericRepository<Member> memberRepository;
        private readonly IGrnericRepository<Membership> membershipRepository;
        private readonly IGrnericRepository<Plan> planRepository;
        private readonly IGrnericRepository<HealthRecord> healthRecordRepository;
        private readonly IGrnericRepository<Booking> bookingRepository;

        public MemberServices(IGrnericRepository<Member> memberRepository, IGrnericRepository<Membership> membershipRepository, IGrnericRepository<Plan> planRepository, IGrnericRepository<HealthRecord> healthRecordRepository, IGrnericRepository<Booking> bookingRecordRepository)
        {
            this.memberRepository = memberRepository;
            this.membershipRepository = membershipRepository;
            this.planRepository = planRepository;
            this.healthRecordRepository = healthRecordRepository;
            this.bookingRepository = bookingRecordRepository;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExists = await memberRepository.AnyAsync(m => m.Email == model.Email, ct);
            var phoneExists = await memberRepository.AnyAsync(m => m.Phone == model.Phone, ct);

            if (emailExists || phoneExists) return false;

            var Member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street,
                },
                HealthRecord = new HealthRecord()
                {
                    Weight = model.HealthRecordViewModel.Weight,
                    Height = model.HealthRecordViewModel.Height,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note
                }
            };

            memberRepository.Add(Member);
            return await memberRepository.CompleteAsync() > 0;
        }

        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var HasFutureSessions = await bookingRepository.AnyAsync(b => b.MemberId == memberId && b.Session.EndDate > DateTime.Now, ct);
            var HasActiveMembership = await membershipRepository.AnyAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now, ct);

            if (HasFutureSessions || HasActiveMembership) return false;

            memberRepository.Delete(memberId);

            var Result = await memberRepository.CompleteAsync();

            return Result > 0;

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await memberRepository.GetAllAsync(false, ct);
            if (!members.Any()) return [];
            var membersViewModel = members.Select(m => new MemberViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString()
            });
            return membersViewModel;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            //Member + Membership + Plan
            var member = await memberRepository.GetByIdAsync(memberId, ct);

            if (member is null) return null;

            var MemberVM = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };

            //MemberShip
            var ActiveMemberShip = await membershipRepository.FirstOrDefaultAsync(
                mb => mb.MemberId == memberId && mb.EndDate > DateTime.Now, false ,ct
            );

            if (ActiveMemberShip is not null)
            {
                var ActivePlan = await planRepository.GetByIdAsync(ActiveMemberShip.PlanId, ct);

                MemberVM.PlanName = ActivePlan?.Name;
                MemberVM.MembershipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                MemberVM.MembershipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
            }

            return MemberVM;

        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var Record = await healthRecordRepository.FirstOrDefaultAsync(r => r.MemberId == memberId, false, ct);

            if (Record is null) return null;

            return new HealthRecordViewModel()
            {
                Weight = Record.Weight,
                Height = Record.Height,
                BloodType = Record.BloodType,
                Note = Record.Note,
            };

        }

        public async Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await memberRepository.GetByIdAsync(memberId, ct);

            if (member is null) return null;

            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Street = member.Address.Street,
                City = member.Address.City,
                BuildingNumber = member.Address.BuildingNumber,
                Photo = member.Photo
            };

        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await memberRepository.GetByIdAsync(id, ct);

            if (member is null) return false;

            if (await memberRepository.AnyAsync(m => m.Email == model.Email && m.Id != id)) return false;
            if (await memberRepository.AnyAsync(m => m.Phone == model.Phone && m.Id != id)) return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.UpdatedAt = DateTime.Now;

            memberRepository.Update(member);

            var Result = await memberRepository.CompleteAsync();

            return Result > 0;
        }
    }
}
