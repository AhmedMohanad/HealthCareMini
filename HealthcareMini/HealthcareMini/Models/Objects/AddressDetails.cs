
using HealthcareMini.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models.Objects
{
    [Owned]
    public class AddressDetails
    {
        [MaxLength(40)]
        public string? Street { get; set; }

        [MaxLength(30)]
        public string? City { get; set; }

        // Stored as string in DB so Province names are readable
        public Province? Province { get; set; }

        [MaxLength(10)]
        public string? ZipCode { get; set; }
    }
}