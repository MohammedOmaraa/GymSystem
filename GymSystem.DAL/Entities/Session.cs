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
                if (DateTime.Now < StartDate)
                    return SessionStatus.Upcoming;

                if (DateTime.Now > EndDate)
                    return SessionStatus.Completed;

                return SessionStatus.Ongoing;
            }
        }

        public int TrainerId { get; set; }

        public Trainer Trainer { get; set; } = null!;

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}