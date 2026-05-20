using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using HealthcareMini.Models.Interfaces;
using HealthcareMini.Models.Objects;



namespace HealthcareMini.DTOs.HealthCareCenterDTO
{
    public class CreateCenterDTO: IBeneficiary
    {

      
        public int Id { get; set; }

      
        public string PasswordHash { get; set; } = string.Empty;

       
        public string Email { get; set; } = string.Empty;

     
        public string Name { get; set; } = string.Empty;

   
        public bool IsActive { get; set; } = true;

      
        public ContactDetails ContactDetails { get; set; } = new ContactDetails();

       
        public AddressDetails AddressDetails { get; set; } = new AddressDetails();

        public UserRole Role { get; set; } = UserRole.HealthCareCenter;


        // Center Informations
        public ICollection<Doctor> Doctors { get; set; } = [];
        public ICollection<Receptionist> Receptionists { get; set; } = [];
        public ICollection<Staff> Staff { get; set; } = [];
        public ICollection<Appointment> Appointments { get; set; } = [];

    }
}
