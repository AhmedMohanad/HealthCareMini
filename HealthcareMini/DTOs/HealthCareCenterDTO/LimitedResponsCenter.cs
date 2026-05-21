using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Objects;

namespace HealthcareMini.DTOs.HealthCareCenterDTO
{
    public class LimitedResponsCenter
    {

        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;


        public string Name { get; set; } = string.Empty;

        public ContactDetails ContactDetails { get; set; } = new ContactDetails();


        public AddressDetails AddressDetails { get; set; } = new AddressDetails();


       
    }
}
