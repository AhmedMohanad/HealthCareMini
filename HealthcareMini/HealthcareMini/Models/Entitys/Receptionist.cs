using HealthcareMini.Models.Interfaces;

namespace HealthcareMini.Models.Entitys
{
    public class Receptionist: User , IEmployee
    {
        //people how responsible for booking appointments and managing patient records
        public double Salary { get; set; }
    }
}
