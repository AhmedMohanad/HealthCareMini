using HealthcareMini.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareMini.Models.Entitys
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // who reserve it
        [Required]
        public int PatientId { get; set; }
        [Required]
        public Patient Patient { get; set; } = new Patient();

        [Required]
        public int DoctorId { get; set; }
        [Required]
        public Doctor Doctor { get; set; } = new Doctor();

        [Required]
        public int HealthCareCenterId { get; set; }
        [Required]
        public HealthCareCenter HealthCareCenter { get; set; } = new HealthCareCenter();

        public DateTime AppointmentDate { get; set; }     // start time 
        public DateTime AppointmentEndTime { get; set; }  // when the appointment will end

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [MaxLength(500)]
        public string? ReasonForVisit { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;






    }
}
