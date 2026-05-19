using HealthcareMini.Models.Interfaces;

namespace HealthcareMini.Models.Entitys
{
    public class Doctor : User, IEmployee
    {
        public double Salary { get; set; }
    }
}
