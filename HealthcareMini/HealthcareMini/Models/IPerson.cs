using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareMini.Models
{
    public interface IPerson
    {
        // this is the main interface for all people in the system, including patients, doctors, and staff.
        // it is made to give more flixibility in the future if we want to add more types of people.


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  

   
        [Required, MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }



    }
}
