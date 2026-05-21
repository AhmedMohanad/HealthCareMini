using HealthcareMini.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models.Entitys
{
    public class Patient : User
    {
        // Patient has medical records
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = [];

        // Patient has appointments
        public ICollection<Appointment> Appointments { get; set; } = [];

        
        // Patients are not employees
    }
}