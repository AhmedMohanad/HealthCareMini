using HealthcareMini.Models.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HealthcareMini.Models
{
    public interface IPerson : IContactable , IAddressable
    {
        // this is the main interface for all people in the system, including patients, doctors, and staff.
        // it is made to give more flexibility in the future if we want to add more types of people.


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  

   
        [Required, MaxLength(30)]
        public string FirstName{ get; set; }

        [MaxLength(30), AllowNull]
        public string LastName{ get; set; }

        public UserRole Role { get; set; }



    }
}
