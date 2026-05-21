// DTOs/MedicalRecord/MedicalRecordRequestDTO.cs
namespace HealthcareMini.DTOs.MedicalRecord
{
    public class MedicalRecordRequestDTO
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int HealthCareCenterId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}