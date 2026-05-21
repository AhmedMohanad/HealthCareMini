// DTOs/MedicalRecord/MedicalRecordResponseDTO.cs
namespace HealthcareMini.DTOs.MedicalRecord
{
    public class MedicalRecordResponseDTO
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int HealthCareCenterId { get; set; }
        public string HealthCareCenterName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? Notes { get; set; }
    }
}