using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.DAL.Entities
{
    [Owned]
    public class Address
    {
        public int BuildingNumber { get; set; }

        public string Street { get; set; } = null!;

        public string City { get; set; } = null!;
    }
}
