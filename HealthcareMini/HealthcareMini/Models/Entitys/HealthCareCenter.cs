using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models.Entitys
{
    public class HealthCareCenter
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(400)]
        public bool IsActive { get; set; } = true;


        // Center Informations
        public ICollection<Doctor> Doctors { get; set; } = [];
        public ICollection<Receptionist> Receptionists { get; set; } = [];
        public ICollection<Staff> Staff { get; set; } = [];
        public ICollection<Appointment> Appointments { get; set; } = [];

    }
}
