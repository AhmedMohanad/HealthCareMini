using HealthcareMini.DTOs.Doctor;
using HealthcareMini.Models.Entitys;

namespace HealthcareMini.Services.DoctorServices
{
    public interface IDoctorService
    {
        Task<Doctor?> GetByIdAsync(int id);
        Task<Doctor?> GetByEmailAsync(string email);
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<IEnumerable<Doctor>> GetDoctorsByCenterAsync(int centerId);
        Task<Doctor> CreateAsync(DoctorRequestDTO dto);
        Task<Doctor?> UpdateAsync(int id, DoctorRequestDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync(int doctorId, DateTime? date = null);
    }
}