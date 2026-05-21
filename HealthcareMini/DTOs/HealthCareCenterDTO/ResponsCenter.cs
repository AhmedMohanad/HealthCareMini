using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Objects;

namespace HealthcareMini.DTOs.HealthCareCenterDTO
{
    public class ResponsCenter
    {

        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;


        public ContactDetails ContactDetails { get; set; } = new ContactDetails();

        public AddressDetails AddressDetails { get; set; } = new AddressDetails();


        // Center Informations
        public ICollection<Doctor> Doctors { get; set; } = [];
        public ICollection<Receptionist> Receptionists { get; set; } = [];
        public ICollection<Staff> Staff { get; set; } = [];
        public ICollection<Appointment> Appointments { get; set; } = [];
    }
}
