using HealthcareMini.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models
{
    public class User : IPerson
    {
        private string phoneNumber = string.Empty;

        // this is the base user model it will handle (admin) only for now 


        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public UserRole Role { get; set; }


       
        public ContentDetails ContactDetails { get; set; } = new ContentDetails();

        // the properties above is from the IPerson interface







    }
}
