using GymSystem.DAL.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.DAL.Entities
{
    public class HealthRecord:BaseEntity
    {
        public decimal Height { get; set; }

        public decimal Weight { get; set; }

        public BloodType BloodType { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }
        
        public int MemberId { get; set; }
       
        public Member Member { get; set; } = null!;
    }
}
