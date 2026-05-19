using HealthcareMini.Models.Interfaces;

namespace HealthcareMini.Models.Entitys
{
    public class Staff : User, IEmployee
    {
        //people who work in the hospital but they are not doctor or receptionist
        public double Salary { get; set; }
    }
}
