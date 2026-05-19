using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models
{
    public interface IAddressable
    {
      
        public AddressDetails Address { get; set; }


    }
}
