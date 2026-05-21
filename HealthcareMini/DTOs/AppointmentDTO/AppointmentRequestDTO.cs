// DTOs/Appointment/AppointmentRequestDTO.cs
using HealthcareMini.Models.Enums;

namespace HealthcareMini.DTOs.Appointment
{
    public class AppointmentRequestDTO
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int HealthCareCenterId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentEndTime { get; set; }
        public string? ReasonForVisit { get; set; }
    }
}