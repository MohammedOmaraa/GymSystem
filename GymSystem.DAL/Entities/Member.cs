using System.ComponentModel.DataAnnotations;

namespace GymSystem.DAL.Entities
{
    public class Member: GymUser
    {
        [Required]
        public string Photo { get; set; } = null!;

        public DateTime JoinDate { get; set; }

        public HealthRecord HealthRecord { get; set; } = null!;
        public ICollection<Membership> Memberships { get; set; } = new HashSet<Membership>();

        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}