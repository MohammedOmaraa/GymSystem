using GymSystem.DAL.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymSystemG03.BLL.ViewModels.MembersViewModels
{
    public class HealthRecordViewModel
    {
        [Range(0.1, 300, ErrorMessage = "Height must be greater than 0")]
        public decimal Height { get; set; }

        [Range(0.1, 500, ErrorMessage = "Weight must be greater than 0")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Blood Type Is Required")]
        public BloodType BloodType { get; set; }
        public string? Note { get; set; } = default!;
    }
}