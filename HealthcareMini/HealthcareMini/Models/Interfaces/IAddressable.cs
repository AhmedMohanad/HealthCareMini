using HealthcareMini.Models.Objects;
using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models.Interfaces
{
    public interface IAddressable
    {
        // this interface will be implemented by any thing who has an address
        public AddressDetails AddressDetails { get; set; }


    }
}
