// DTOs/Appointment/AppointmentResponseDTO.cs
using HealthcareMini.Models.Enums;

namespace HealthcareMini.DTOs.Appointment
{
    public class AppointmentResponseDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public int HealthCareCenterId { get; set; }
        public string HealthCareCenterName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentEndTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? ReasonForVisit { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}