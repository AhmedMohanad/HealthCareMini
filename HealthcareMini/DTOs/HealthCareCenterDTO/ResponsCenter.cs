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
        public ICollection<HealthcareMini.Models.Entitys.Doctor> Doctors { get; set; } = [];
        public ICollection<HealthcareMini.Models.Entitys.Receptionist> Receptionists { get; set; } = [];
        public ICollection<HealthcareMini.Models.Entitys.Staff> Staff { get; set; } = [];
        public ICollection<HealthcareMini.Models.Entitys.Appointment> Appointments { get; set; } = [];
    }
}
