using HealthcareMini.Models.Enums;
using HealthcareMini.Models.Objects;

namespace HealthcareMini.DTOs
{
    public class UserDto
    {
        
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        //HealthCareCenter only fields
        public string? Name { get; set; }

        
        public double? Salary { get; set; }
        public List<int>? HealthCareCenterIds { get; set; } // Pass IDs to link centers

        //octors only fields
        public string? Specialization { get; set; }

        //staff only fields
        public string? JobTitle { get; set; }

        
        public ContactDetails? ContactDetails { get; set; }
        public AddressDetails? AddressDetails { get; set; }
    }
}