using HealthcareMini.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models.Entitys
{
    public class Staff : User, IEmployee
    {
        //people who work in the hospital but they are not doctor or receptionist
        public double Salary { get; set; }

        [MaxLength(100)]
        public String JobTitle { get; set; } = string.Empty;
        [Required]
        public ICollection<HealthCareCenter> HealthCareCenters { get; set; } = [];
    }
}
