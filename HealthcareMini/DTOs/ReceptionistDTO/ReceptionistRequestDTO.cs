// DTOs/Receptionist/ReceptionistRequestDTO.cs
using HealthcareMini.Models.Objects;

namespace HealthcareMini.DTOs.Receptionist
{
    public class ReceptionistRequestDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public ContactDetails ContactDetails { get; set; } = new ContactDetails();
        public AddressDetails AddressDetails { get; set; } = new AddressDetails();
        public double Salary { get; set; }
        public List<int> HealthCareCenterIds { get; set; } = [];
    }
}