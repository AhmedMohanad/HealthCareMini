using HealthcareMini.Models.Interfaces;

namespace HealthcareMini.Models.Entitys
{
    public class Receptionist: User , IEmployee
    {
        public double Salary { get; set; }
    }
}
