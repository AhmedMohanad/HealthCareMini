// DTOs/Admin/AdminResponseDTO.cs
using HealthcareMini.Models.Objects;
using HealthcareMini.Models.Enums;

namespace HealthcareMini.DTOs.Admin
{
    public class AdminResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public ContactDetails ContactDetails { get; set; } = new ContactDetails();
        public AddressDetails AddressDetails { get; set; } = new AddressDetails();
        public UserRole Role { get; set; }
    }
}