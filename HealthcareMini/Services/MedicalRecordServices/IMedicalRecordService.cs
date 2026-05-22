using HealthcareMini.DTOs.MedicalRecord;
using HealthcareMini.Models.Entitys;

namespace HealthcareMini.Services.MedicalRecordServices
{
    public interface IMedicalRecordService
    {
        Task<MedicalRecord?> GetByIdAsync(int id);
        Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<MedicalRecord>> GetByDoctorIdAsync(int doctorId);
        Task<MedicalRecord> CreateAsync(MedicalRecordRequestDTO dto);
        Task<MedicalRecord?> UpdateAsync(int id, MedicalRecordRequestDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}