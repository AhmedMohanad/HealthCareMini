using HealthcareMini.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models.Entitys
{
    public class Receptionist: User , IEmployee
    {
        //people how responsible for booking appointments and managing patient records
        public double Salary { get; set; }


        [Required]
        public ICollection<HealthCareCenter> HealthCareCenters { get; set; } = [];
    }
}
