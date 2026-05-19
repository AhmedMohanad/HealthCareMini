using HealthcareMini.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareMini.Models.Objects
{
    public class AddressDetails
    {
        //this is an address details model that will be used to store the address for each user

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Street { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public Province Province { get; set; }

        [Required]
        [MaxLength(10)]
        public string ZipCode { get; set; } = string.Empty;
    }
}
