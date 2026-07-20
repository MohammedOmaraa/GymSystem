using GymSystem.DAL.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymSystem.DAL.Entities
{
    public class Session:BaseEntity
    {
        public string Description { get; set; } = null!;

        public int Capacity { get; set; }
        
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [NotMapped]
        public SessionStatus Status
        {
            get
            {
                var now = DateTime.UtcNow;

                if (now < StartDate)
                    return SessionStatus.Upcoming;

                if (now >= StartDate && now < EndDate)
                    return SessionStatus.Ongoing;

                return SessionStatus.Completed;
            }
        }

        public int TrainerId { get; set; }

        public Trainer Trainer { get; set; } = null!;

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}