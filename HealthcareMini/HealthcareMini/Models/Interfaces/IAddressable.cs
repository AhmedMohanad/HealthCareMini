using HealthcareMini.Models.Objects;
using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models.Interfaces
{
    public interface IAddressable
    {
      
        public AddressDetails Address { get; set; }


    }
}
