using HealthcareMini.DTOs.Patient;
using HealthcareMini.Models.Entitys;

namespace HealthcareMini.Services.PatientServices
{
    public interface IPatientService
    {
        Task<Patient?> GetByIdAsync(int id);
        Task<Patient?> GetByEmailAsync(string email);
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient> CreateAsync(PatientRequestDTO dto);
        Task<Patient?> UpdateAsync(int id, PatientRequestDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(int patientId);
        Task<IEnumerable<MedicalRecord>> GetPatientMedicalRecordsAsync(int patientId);
    }
}