namespace HealthcareMini.Models.Entitys
{
    public class Patient : User
    {
        //  each patient can have many medical records 
        public List<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    }
}
