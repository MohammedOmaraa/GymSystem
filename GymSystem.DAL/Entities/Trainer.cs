using GymSystem.DAL.Entities.Enums;

namespace GymSystem.DAL.Entities
{
    public class Trainer:GymUser
    {
        public Specialty Specialty { get; set; }

        public DateTime HireDate { get; set; }

        public ICollection<Session> Sessions { get; set; }  = new HashSet<Session>();
    }
}
