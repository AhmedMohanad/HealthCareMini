using HealthcareMini.Models.Enums;
using HealthcareMini.Models.Interfaces;
using HealthcareMini.Models.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareMini.Models.Entitys
{
    public class HealthCareCenter : IContactable, IAddressable , IBeneficiary
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public ContactDetails ContactDetails { get; set; } = new ContactDetails();

        [Required]
        public AddressDetails AddressDetails { get; set; } = new AddressDetails();

        public UserRole Role { get; set; }


        // Center Informations
        public ICollection<Doctor> Doctors { get; set; } = [];
        public ICollection<Receptionist> Receptionists { get; set; } = [];
        public ICollection<Staff> Staff { get; set; } = [];
        public ICollection<Appointment> Appointments { get; set; } = [];
        
    }
}
