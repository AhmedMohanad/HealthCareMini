// DTOs/Appointment/AppointmentStatusUpdateDTO.cs
using HealthcareMini.Models.Enums;

namespace HealthcareMini.DTOs.Appointment
{
    public class AppointmentStatusUpdateDTO
    {
        public AppointmentStatus Status { get; set; }
        public string? Reason { get; set; }
    }
}