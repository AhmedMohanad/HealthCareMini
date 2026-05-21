// DTOs/Doctor/DoctorResponseDTO.cs
using HealthcareMini.Models.Objects;
using HealthcareMini.Models.Enums;
using HealthcareMini.DTOs.HealthCareCenterDTO;

namespace HealthcareMini.DTOs.Doctor
{
    public class DoctorResponseDTO
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
        public double Salary { get; set; }
        public bool IsActive { get; set; }
        public string Specialization { get; set; } = string.Empty;
        public List<LimitedResponsCenter> HealthCareCenters { get; set; } = [];
    }
}