using HealthcareMini.Models.Interfaces;

namespace HealthcareMini.Models.Entitys
{
    public class Staff : User, IEmployee
    {
        public double Salary { get; set; }
    }
}
