using HealthcareMini.Models.Objects;

namespace HealthcareMini.DTOs.Staff
{
    public class UpdateStaffProfileDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public ContactDetails? ContactDetails { get; set; }
        public AddressDetails? AddressDetails { get; set; }
        public string? JobTitle { get; set; }
    }
}