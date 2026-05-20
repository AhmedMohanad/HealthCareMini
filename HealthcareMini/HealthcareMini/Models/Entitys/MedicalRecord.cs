using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareMini.Models.Entitys
{
    public class MedicalRecord
    {
        // this is the record that will be used to store the medical history of the patient and the doctor who created it


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        // which doctor created this record and which patient is this record for
        [Required]
        public Doctor Doctor { get; set; } = new Doctor();

        public int DoctorId { get; set; }


        // which patient is this record for
        [Required]
        public Patient Patient { get; set; } = new Patient();
        public int PatientId { get; set; }

        [Required]
        public HealthCareCenter HealthCareCenter { get; set; } = new HealthCareCenter();
        public int HealthCareCenterId { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? Notes { get; set; }

    }
}
