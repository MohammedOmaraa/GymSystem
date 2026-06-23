using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.DAL.Entities
{
    [Owned]
    public class Address
    {
        public int BuildingNumber { get; set; }

        [Required, MaxLength(30)]
        public string Street { get; set; } = null!;

        [Required, MaxLength(30)]
        public string City { get; set; } = null!;
    }
}
