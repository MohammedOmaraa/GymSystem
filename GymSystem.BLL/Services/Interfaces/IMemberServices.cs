using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.MembersViewModels;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IMemberServices
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default);
        Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default);
        Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default);

    }
}
