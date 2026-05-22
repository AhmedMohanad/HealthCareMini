using HealthcareMini.Models.Entitys;

namespace HealthcareMini.Services.HealthCareCenterServices
{
    public interface IHealthCareCenterAppointmentService
    {
        Task<Appointment?> AddAppointmentAsync(int centerId, Appointment appointment);
        Task<IEnumerable<Appointment>> AddAppointmentsAsync(int centerId, IEnumerable<Appointment> appointments);
        Task<bool> RemoveAppointmentFromCenterAsync(int centerId, int appointmentId);
    }
}