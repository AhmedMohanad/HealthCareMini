// DTOs/Appointment/AppointmentRescheduleDTO.cs
namespace HealthcareMini.DTOs.Appointment
{
    public class AppointmentRescheduleDTO
    {
        public DateTime NewDate { get; set; }
        public DateTime NewEndTime { get; set; }
        public string? Reason { get; set; }
    }
}