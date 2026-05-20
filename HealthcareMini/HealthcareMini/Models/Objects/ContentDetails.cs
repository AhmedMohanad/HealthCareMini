

using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Models.Objects
{
    [Owned]
    public class ContactDetails
    {
        // Phone numbers stored as JSON array in a single column: ["07701234","07709876"]
        public List<string> PhoneNumbers { get; set; } = [];
    }
}