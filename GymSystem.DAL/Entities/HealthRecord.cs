using System.ComponentModel.DataAnnotations;

namespace GymSystem.DAL.Entities
{
    public class HealthRecord:BaseEntity
    {
        public decimal Height { get; set; }

        public decimal Weight { get; set; }


        [Required, MaxLength(5)]
        public string BloodType { get; set; } = null!;

        [MaxLength(500)]
        public string? Note { get; set; }
        
        public int MemberId { get; set; }
       
        public Member Member { get; set; } = null!;
    }
}
