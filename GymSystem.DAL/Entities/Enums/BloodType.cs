using System.ComponentModel.DataAnnotations;

namespace GymSystem.DAL.Entities.Enums
{
    public enum BloodType
    {
        [Display(Name = "A+")]
        APositive = 1,

        [Display(Name = "A-")]
        ANegative = 2,

        [Display(Name = "B+")]
        BPositive = 3,

        [Display(Name = "B-")]
        BNegative = 4,

        [Display(Name = "AB+")]
        ABPositive = 5,

        [Display(Name = "AB-")]
        ABNegative = 6,

        [Display(Name = "O+")]
        OPositive = 7,

        [Display(Name = "O-")]
        ONegative = 8
    }
}