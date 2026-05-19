using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareMini.Models.Objects
{
    public class ContentDetails
    {
        // this is the content details model for every user

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required, MaxLength(70)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public List<string >PhoneNumber { get; set; } = new List<string>();
    }
}
