using HealthcareMini.Models.Enums;
using HealthcareMini.Models.Interfaces;
using HealthcareMini.Models.Objects;
using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models.Entitys
{
    public class User : IPerson, IBeneficiary
    {
        

        // this is the base user model it will handle (admin) only for now 


        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;


        public DateTime DateOfBirth { get; set; }

        public DateTime DateOfRegistration { get; set; } = DateTime.Now;

        // the properties above is from the IPerson interface


        [Required]
        public ContactDetails ContactDetails { get; set; } = new ContactDetails();

        // the properties above is from the IContactable interface
        public AddressDetails AddressDetails { get; set; } = new AddressDetails();

        // the properties above is from the IAddressable interface







    }
}
