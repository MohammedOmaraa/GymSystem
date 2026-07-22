using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAttachmentServices attachmentServices;

        public MemberServices(IUnitOfWork unitOfWork, IAttachmentServices attachmentServices)
        {
            this.unitOfWork = unitOfWork;
            this.attachmentServices = attachmentServices;
        }

        public async Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var memberRepository = unitOfWork.GetRepository<Member>();

            var emailExists = await memberRepository.AnyAsync(m => m.Email == model.Email, ct);

            if (emailExists)
                return Result.Validation("Email already exists.");

            var phoneExists = await memberRepository.AnyAsync(m => m.Phone == model.Phone, ct);

            if (phoneExists)
                return Result.Validation("Phone already exists.");

            var Member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                //Photo = await attachmentServices.UploadAsync(model.Image.OpenReadStream(), model.Image.FileName, "MemberPictures", ct),
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

            var uploadResult = await attachmentServices.UploadAsync(model.Image.OpenReadStream(), model.Image.FileName, "MemberPictures", ct);

            if (!uploadResult.IsSuccess)
                return Result.Validation(uploadResult.Message!);

            Member.Photo = uploadResult.Value!;

            if (string.IsNullOrEmpty(Member.Photo))
            {
                return Result.Failure("Failed to upload member photo.");
            }

            unitOfWork.GetRepository<Member>().Add(Member);
            var rows = await unitOfWork.CompeleteAsync();

            if (rows <= 0)
            {
                attachmentServices.Delete(Member.Photo!, "MemberPictures");

                return Result.Failure("Failed to save member.");
            }
            return Result.Success();
        }

        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var HasFutureSessions = await unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == memberId && b.Session.EndDate > DateTime.Now, ct);
            var HasActiveMembership = await unitOfWork.GetRepository<Membership>().AnyAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now, ct);

            if (HasFutureSessions || HasActiveMembership) return false;

            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member == null) return false;

            if (member.Photo is not null)
                attachmentServices.Delete(member.Photo, "MemberPictures");
            

            unitOfWork.GetRepository<Member>().Delete(memberId);    
            var Result = await unitOfWork.CompeleteAsync();

            return Result > 0;

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAllAsync(false, ct);
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
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

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
            var ActiveMemberShip = await unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(
                mb => mb.MemberId == memberId && mb.EndDate > DateTime.Now, false ,ct
            );

            if (ActiveMemberShip is not null)
            {
                var ActivePlan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(ActiveMemberShip.PlanId, ct);

                MemberVM.PlanName = ActivePlan?.Name;
                MemberVM.MembershipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                MemberVM.MembershipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
            }

            return MemberVM;

        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var Record = await unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(r => r.MemberId == memberId, false, ct);

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
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

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
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);

            if (member is null) return false;

            if (await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != id)) return false;
            if (await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != id)) return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepository<Member>().Update(member);

            var Result = await unitOfWork.CompeleteAsync();

            return Result > 0;
        }
    }
}
