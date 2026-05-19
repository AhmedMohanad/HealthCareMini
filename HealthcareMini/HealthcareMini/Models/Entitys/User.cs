using HealthcareMini.Models.Enums;
using HealthcareMini.Models.Interfaces;
using HealthcareMini.Models.Objects;
using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models.Entitys
{
    public class User : IPerson
    {
        private string phoneNumber = string.Empty;

        // this is the base user model it will handle (admin) only for now 


        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string DOB { get; set; } = string.Empty;

        public DateTime Date = DateTime.Now;

        // the properties above is from the IPerson interface


        public ContentDetails ContactDetails { get; set; } = new ContentDetails();

        // the properties above is from the IContactable interface
        public AddressDetails AddressDetails { get; set; } = new AddressDetails();

        // the properties above is from the IAddressable interface







    }
}
