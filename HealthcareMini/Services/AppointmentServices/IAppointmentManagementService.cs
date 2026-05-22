using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;

namespace HealthcareMini.Services.AppointmentServices
{
    public interface IAppointmentManagementService
    {
        Task<Appointment?> CheckInAsync(int appointmentId);
        Task<Appointment?> CompleteAsync(int appointmentId);
        Task<Appointment?> CancelAsync(int appointmentId, string? cancelReason = null);
        Task<Appointment?> MarkAsNoShowAsync(int appointmentId);
        Task<Appointment?> RescheduleAsync(int appointmentId, DateTime newDate, DateTime newEndTime);
        Task<Appointment?> RescheduleWithReasonAsync(int appointmentId, DateTime newDate, DateTime newEndTime, string rescheduleReason);
        Task<bool> CheckDoctorAvailabilityAsync(int doctorId, DateTime startTime, DateTime endTime, int? excludeAppointmentId = null);
        Task<List<TimeSlot>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date, int durationMinutes = 30);
        Task<Appointment?> GetAppointmentByIdAsync(int appointmentId);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId, DateTime? date = null);
        Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(int centerId, AppointmentStatus status);
        Task<int> AutoCancelNoShowsAsync();
    }
}