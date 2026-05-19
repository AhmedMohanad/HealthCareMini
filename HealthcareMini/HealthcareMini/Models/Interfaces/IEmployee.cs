using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareMini.Models.Interfaces
{
    public interface IEmployee
    {
        // this interface will be implemented by any one how get salary 
        public Double Salary { get; set; }


    }
}
