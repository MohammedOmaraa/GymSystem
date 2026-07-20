using GymSystem.DAL.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.DAL.Entities
{
    public class GymUser:BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public Address Address { get; set; } = null!;
    }
}
