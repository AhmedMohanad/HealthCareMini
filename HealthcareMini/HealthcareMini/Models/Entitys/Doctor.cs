using HealthcareMini.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models.Entitys
{
    public class Doctor : User, IEmployee
    {
        public double Salary { get; set; }

        public bool IsActive { get; set; } = true;
        
        [Required,MaxLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Required]
        public ICollection<HealthCareCenter> HealthCareCenters { get; set; } = [];
    }
}
